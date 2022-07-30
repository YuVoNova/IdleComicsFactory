using UnityEngine;

public class ComicBook : MonoBehaviour
{
    [SerializeField]
    private GameObject image;
    [SerializeField]
    private Renderer bookRenderer;
    [SerializeField]
    private Material material;

    public void ScriptedConvert()
    {
        image.SetActive(true);

        bookRenderer.materials[0] = material;
    }
}
