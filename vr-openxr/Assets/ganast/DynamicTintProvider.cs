using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(XRBaseInteractable))]
public class DynamicTintProvider: MonoBehaviour {

    [SerializeField]
    private Color color = new Color(1.0f, 0.5f, 0.15f, 1.0f);

    [SerializeField]
    private float speed = 2.0f;

    [SerializeField]
    private bool animate = true;

    [SerializeField]
    private bool tintWhenSelected = false;

    [SerializeField]
    private Material baseMaterial;

    private Material tint;
    private XRBaseInteractable target;

    private bool tinting = false;
    private bool selected = false;
    private bool hovered = false;

    private float timebase = 0;

    public void Start() {
        
        Renderer renderer = GetComponent<Renderer>();
        
        List<Material> materials = new List<Material>();
        renderer.GetMaterials(materials);
        tint = baseMaterial != null ? new Material(baseMaterial) : new Material(Shader.Find("HDRP/Unlit"));
        tint.color = Color.clear;
        materials.Add(tint);
        renderer.SetMaterials(materials);

        target = GetComponent<XRBaseInteractable>();
    }

    public void Update() {

        if (target.isSelected && !selected) {
            // selected...
            if (!tintWhenSelected) {
                tint.color = Color.clear;
            }
            selected = true;
        }
        else if (!target.isSelected && selected) {
            // deselected...
            if (!hovered) {
                tint.color = Color.clear;
            }
            else if (!tintWhenSelected) {
                timebase = Time.time;
            }
            selected = false;
        }

        if (target.isHovered && !hovered) {
            // hovered...
            timebase = Time.time;
            hovered = true;
        }
        else if (!target.isHovered && hovered) {
            // unhovered...
            if (!selected || !tintWhenSelected) {
                tint.color = Color.clear;
            }
            hovered = false;
        }

        tinting = (hovered && !selected) || (selected && tintWhenSelected);

        if (tinting) {
            if (animate) {
                tint.color = color * Mathf.PingPong((Time.time - timebase) * speed, 1.0f);
            }
            else {
                tint.color = color;
            }
        }
    }
}
