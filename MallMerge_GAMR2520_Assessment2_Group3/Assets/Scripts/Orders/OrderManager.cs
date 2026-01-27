using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles customer orders:
/// - Picks 1 to 3 requested items (max 1 per chest family).
/// - Shows those items in the Request Banner order slots.
/// - Checks the grid for matching items (family + level).
/// - When found: removes the item from grid, plays success sound, clears the slot.
/// - When all delivered: rewards stars (3/6/10), changes character sprite, generates a new order.
/// </summary>
public class OrderManager : MonoBehaviour
{
    [Header("Scene References")]
    [Tooltip("Your existing GridManager that created the 5x5 tiles.")]
    public GridManager gridManager;

    [Tooltip("Optional: if you're using pooling, assign your ItemPool here. If null, we Destroy() delivered items.")]
    public ItemPool itemPool;

    [Tooltip("Drag your 3 OrderSlot Image objects here (OrderSlot1_image, OrderSlot2_image, OrderSlot3_image).")]
    public Image[] orderSlots;

    [Tooltip("Drag your Currency Counter Text here (the number next to the star).")]
    public TMP_Text starCounterText;


    [Tooltip("Your DisplayCharacter component (script) that controls the Character_Image.")]
    public DisplayCharacter displayCharacter;

    [Tooltip("AudioSource that plays when an order item is successfully delivered.")]
    public AudioSource successAudio;

    [Header("Data (Chains + Characters)")]
    [Tooltip("Assign ALL 5 MergeChainData assets here (Fruit, Dessert, Wardrobe, Makeup, Tech).")]
    public List<MergeChainData> chains;

    [Tooltip("Character sprites to rotate through after completing an order.")]
    public List<Sprite> characterSprites;

    [Header("Order Rules")]
    [Tooltip("Min number of items in an order.")]
    public int minOrderSize = 1;

    [Tooltip("Max number of items in an order (you said 1-3).")]
    public int maxOrderSize = 3;

    [Tooltip("Min merge level requested (0 = first sprite).")]
    public int minRequestedLevel = 0;

    [Tooltip("Max merge level requested. If -1, uses each chain's MaxLevel.")]
    public int maxRequestedLevelOverride = -1;

    // --- internal state ---
    [Serializable]
    public class OrderRequest
    {
        public ChestFamily family;
        public int level;
        public bool completed;
    }

    private readonly List<OrderRequest> activeRequests = new List<OrderRequest>();
    private Dictionary<ChestFamily, MergeChainData> chainLookup;

    private int stars = 0;

    private void Awake()
    {
        // Build a lookup so we can quickly get the chain for any family.
        chainLookup = new Dictionary<ChestFamily, MergeChainData>();

        if (chains != null)
        {
            foreach (var c in chains)
            {
                if (c == null) continue;
                chainLookup[c.family] = c;
            }
        }

        // Hide order slots at start (clean UI).
        HideAllSlots();

        // Initialize star counter display.
        RefreshStarUI();
    }

    private void Start()
    {
        // Make the first order as soon as game starts.
        GenerateNewOrder();
    }

    /// <summary>
    /// Creates a new order of 1-3 items, each from a different chest family.
    /// Displays them in the order slots.
    /// </summary>
    public void GenerateNewOrder()
    {
        activeRequests.Clear();
        HideAllSlots();

        if (gridManager == null)
        {
            Debug.LogError("OrderManager: gridManager is not assigned.");
            return;
        }

        if (orderSlots == null || orderSlots.Length == 0)
        {
            Debug.LogError("OrderManager: orderSlots not assigned.");
            return;
        }

        // Decide how many items this customer wants (1-3)
        int orderSize = UnityEngine.Random.Range(minOrderSize, maxOrderSize + 1);

        // Build list of available families
        List<ChestFamily> families = new List<ChestFamily>()
        {
            ChestFamily.FruitGreen,
            ChestFamily.DessertYellow,
            ChestFamily.WardrobePurple,
            ChestFamily.MakeupPink,
            ChestFamily.TechBlue
        };

        // Shuffle-ish: pick random unique families
        for (int i = 0; i < orderSize; i++)
        {
            if (families.Count == 0) break;

            int famIndex = UnityEngine.Random.Range(0, families.Count);
            ChestFamily fam = families[famIndex];
            families.RemoveAt(famIndex);

            // Choose a requested level
            int maxLvl = GetMaxLevelForFamily(fam);
            int minLvl = Mathf.Clamp(minRequestedLevel, 0, maxLvl);
            int chosenLvl = UnityEngine.Random.Range(minLvl, maxLvl + 1);

            activeRequests.Add(new OrderRequest
            {
                family = fam,
                level = chosenLvl,
                completed = false
            });
        }

        // Show in UI slots
        for (int i = 0; i < orderSlots.Length; i++)
        {
            if (i >= activeRequests.Count)
            {
                orderSlots[i].gameObject.SetActive(false);
                continue;
            }

            var req = activeRequests[i];
            Sprite s = GetSpriteForRequest(req.family, req.level);
            Debug.Log($"Order slot {i}: {req.family} level {req.level} -> sprite = {(s ? s.name : "NULL")}");

            orderSlots[i].sprite = s;
            orderSlots[i].enabled = (s != null); // if missing sprite, still show object but disable image draw
            orderSlots[i].gameObject.SetActive(true);
        }

        // Change to a new character right when a new order appears (optional feel-good)
        ChangeCharacterRandom();
    }

    /// <summary>
    /// Call this after a swipe move OR after spawning an item.
    /// It scans the grid and delivers any matching requested items it finds.
    /// </summary>
    public void CheckForDeliveries()
    {
        if (gridManager == null) return;
        if (activeRequests.Count == 0) return;

        // Scan all tiles for items
        for (int y = 0; y < gridManager.rows; y++)
        {
            for (int x = 0; x < gridManager.columns; x++)
            {
                TileUI t = gridManager.GetTileUI(x, y);
                if (t == null) continue;

                GameObject itemGO = t.currentItem;
                if (itemGO == null) continue;

                MergeItem mi = itemGO.GetComponent<MergeItem>();
                if (mi == null) continue;

                // Try match against any request not completed
                int requestIndex = FindMatchingRequestIndex(mi.family, mi.level);
                if (requestIndex == -1) continue;

                // Deliver!
                DeliverRequestAtIndex(requestIndex, t, itemGO);

                // After delivering one item, continue scanning (there might be more)
            }
        }

        // If everything completed -> payout + new order
        if (AllRequestsCompleted())
        {
            PayoutAndNextCustomer();
        }
    }

    // -------------------- helpers --------------------

    private int FindMatchingRequestIndex(ChestFamily fam, int lvl)
    {
        for (int i = 0; i < activeRequests.Count; i++)
        {
            var req = activeRequests[i];
            if (req.completed) continue;
            if (req.family == fam && req.level == lvl)
                return i;
        }
        return -1;
    }

    private void DeliverRequestAtIndex(int requestIndex, TileUI tile, GameObject itemGO)
    {
        // Mark request completed
        activeRequests[requestIndex].completed = true;

        // Hide/remove the matching UI slot
        if (orderSlots != null && requestIndex < orderSlots.Length)
        {
            orderSlots[requestIndex].gameObject.SetActive(false);
        }

        // Play success sound
        if (successAudio != null)
        {
            successAudio.Play();
        }

        // Remove item from tile (VERY important so it doesn’t overlap)
        tile.currentItem = null;

        // Return item to pool or destroy
        if (itemPool != null)
        {
            // Your pool likely uses family-based queues.
            // If your ItemPool.Return signature differs, tell me and I’ll adjust.
            itemPool.Return(activeRequests[requestIndex].family, itemGO);
        }
        else
        {
            Destroy(itemGO);
        }
    }

    private bool AllRequestsCompleted()
    {
        if (activeRequests.Count == 0) return false;
        for (int i = 0; i < activeRequests.Count; i++)
            if (!activeRequests[i].completed) return false;
        return true;
    }

    private void PayoutAndNextCustomer()
    {
        int completedCount = activeRequests.Count;

        // You asked:
        // 1 item = 3 stars
        // 2 items = 6 stars
        // 3 items = 10 stars
        int reward = 0;
        if (completedCount == 1) reward = 3;
        else if (completedCount == 2) reward = 6;
        else if (completedCount >= 3) reward = 10;

        stars += reward;
        RefreshStarUI();

        // New order + new character
        GenerateNewOrder();
    }

    private void RefreshStarUI()
    {
        if (starCounterText != null)
        {
            starCounterText.text = stars.ToString();
        }
    }

    private void HideAllSlots()
    {
        if (orderSlots == null) return;
        for (int i = 0; i < orderSlots.Length; i++)
        {
            if (orderSlots[i] == null) continue;
            orderSlots[i].sprite = null;
            orderSlots[i].gameObject.SetActive(false);
        }
    }

    private Sprite GetSpriteForRequest(ChestFamily fam, int lvl)
    {
        MergeChainData chain;
        if (!chainLookup.TryGetValue(fam, out chain) || chain == null)
            return null;

        return chain.GetSprite(lvl);
    }

    private int GetMaxLevelForFamily(ChestFamily fam)
    {
        MergeChainData chain;
        if (!chainLookup.TryGetValue(fam, out chain) || chain == null)
            return 0;

        int maxLvl = chain.MaxLevel;

        // If you set an override, clamp to that
        if (maxRequestedLevelOverride >= 0)
            maxLvl = Mathf.Min(maxLvl, maxRequestedLevelOverride);

        return Mathf.Max(0, maxLvl);
    }

    private void ChangeCharacterRandom()
    {
        if (displayCharacter == null) return;
        if (characterSprites == null || characterSprites.Count == 0) return;

        Sprite s = characterSprites[UnityEngine.Random.Range(0, characterSprites.Count)];
        displayCharacter.DisplayImage(s);
    }
   
}
