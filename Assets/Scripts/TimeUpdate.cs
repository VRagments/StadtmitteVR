using UnityEngine;
using UnityEngine.UI;

namespace LNDW {
    public class TimeUpdate : MonoBehaviour {

        private SceneRun sceneRun;
        private Text time;

	    void Awake() {
            sceneRun = FindObjectOfType<SceneRun> ();
            time = GetComponent<Text> ();
        }

   	    void Update () {
            if (sceneRun != null) {
                int min = (int)sceneRun.CurrentTime / 60;
                int sec = (int)sceneRun.CurrentTime  - min * 60;
                time.text = string.Format ("{0:00}:{1:00}", min, sec);
            }
	      }
    }
}
