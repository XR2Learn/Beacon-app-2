using TMPro;

using UnityEngine;

public class TooltipManager: MonoBehaviour  {

    [SerializeField]
    private GameObject tooltip;

    private TMP_Text tooltipText;

    private bool frozen = false;

    private static TooltipManager inst = null;

    public static TooltipManager GetTooltipManager() {
        return inst;
    }

    public void Awake() {
        tooltipText = GetComponentInChildren<TMP_Text>();
        inst = this;
    }

    public void SetTooltipText(string text) {
        if (!frozen) {
            tooltipText.SetText(text);
        }
    }

    public void SetTooltipVisible(bool visible) {
        if (!frozen) {
            tooltip.SetActive(visible);
        }
    }

    public void Freeze() {
        frozen = true;
    }

    public void Unfreeze() {
        frozen = false;
    }

    public void LookAt(Transform transform) {
        tooltip.transform.LookAt(transform);
    }
}
