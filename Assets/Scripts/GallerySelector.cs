using TMPro;
using UnityEngine;

public class GallerySelector : MonoBehaviour
{
    [System.Serializable]
    public class ArtSet
    {
        public string title;
        public Texture2D[] images;
    }

    [Header("Colecciones")]
    public ArtSet[] sets;

    [Header("Lienzos (los Quads donde se muestra cada imagen)")]
    public Renderer[] slots;

    [Tooltip("ON: si el lienzo es hijo de un objeto 'Obra' (con el marco), se oculta ese objeto entero.")]
    public bool hideParentWhenEmpty = true;

    [Header("Opcional")]
    public TMP_Text titleLabel;
    public int startingSet = 0;

    public int CurrentSet { get; private set; } = -1;

    void Start()
    {
        Show(startingSet);
    }

    /// Se llama desde el On Click de los botones del panel (con 0, 1 o 2).
    public void Show(int index)
    {
        if (sets == null || index < 0 || index >= sets.Length) return;
        CurrentSet = index;

        Texture2D[] images = sets[index].images;
        for (int i = 0; i < slots.Length; i++)
        {
            bool hasImage = images != null && i < images.Length && images[i] != null;

            GameObject target = slots[i].gameObject;
            if (hideParentWhenEmpty && slots[i].transform.parent != null)
                target = slots[i].transform.parent.gameObject;
            target.SetActive(hasImage);

            // material = copia propia de este lienzo, así no se pisan
            if (hasImage) slots[i].material.mainTexture = images[i];
        }

        if (titleLabel != null) titleLabel.text = sets[index].title;
    }

    /// cuarto botón "Sorprendeme" que elige una colección random
    public void ShowRandom()
    {
        if (sets == null || sets.Length == 0) return;
        int next = Random.Range(0, sets.Length);
        if (sets.Length > 1 && next == CurrentSet) next = (next + 1) % sets.Length;
        Show(next);
    }
}
