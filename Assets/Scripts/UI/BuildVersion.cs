using TMPro;
using UnityEngine;
using Zenject;

public class BuildVersion : IInitializable
{
    [Inject(Id = "BuildVersionText")]
    private readonly TMP_Text _buildVersionText;

    public void Initialize()
    {
        _buildVersionText.text = "v " + Application.version;
    }
}