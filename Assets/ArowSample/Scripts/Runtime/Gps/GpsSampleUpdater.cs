using ArowMain.Runtime.GPS;
using UnityEngine;
using UnityEngine.UI;

namespace ArowSample.Scripts.Runtime.GPSSample
{
public class GpsSampleUpdater : MonoBehaviour
{
    private LocationManager _locationManager = null;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text cautionText;

    private void Start()
    {
        _locationManager = GetComponent<LocationManager>();
    }

    void Update()
    {
        text.text = _locationManager.Started.ToString()
                    + "\n" + "lat:" + _locationManager.Latitude.ToString()
                    + "\n" + "lng:" + _locationManager.Longitude.ToString();
#if UNITY_EDITOR
        cautionText.text = "UnityEditorでは確認できません。";
#else
        cautionText.text = "GPS機能を有効にすると確認できます。";
#endif
    }
}
}
