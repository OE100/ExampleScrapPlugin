using System.Collections;
using System.Collections.Generic;
using LethalLib;
using UnityEngine;

namespace ExampleScrapPlugin.custom
{
    // reusing my original monkey item with sound and adding light control
    public class DiscoMonkey : MonkeyWithSound
    {
        private Light light;
        private IEnumerator current;
        private Renderer renderer;
        
        static List<Color> from = new List<Color>
        {
            Color.blue,
            Color.cyan,
            Color.green,
            Color.magenta,
            Color.red,
            Color.yellow
        };
        
        public override void Start()
        {
            ExamplePlugin.Log.LogDebug("DiscoMonkey started");
            base.Start();
            light = gameObject.GetComponentInChildren<Light>();
            renderer = gameObject.GetComponentInChildren<Renderer>();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            ExamplePlugin.Log.LogDebug("DiscoMonkey activated");
            base.ItemActivate(used, buttonDown);
            SwitchLight();
        }

        private void SwitchLight()
        {
            ExamplePlugin.Log.LogDebug("Switching light");
            ExamplePlugin.Log.LogDebug($"Switched light {(light.enabled ? "on" : "off")}");
            light.enabled = !light.enabled;
            if (light.enabled)
            {
                current = ChangeColor();
                StartCoroutine(current);
            }
            else if (current != null)
            {
                StopCoroutine(current);
                current = null;
            }
        }

        private IEnumerator ChangeColor()
        {
            ExamplePlugin.Log.LogDebug("Starting color change coroutine");
            
            Color initial = PickRandomPresetColor();
            light.color = initial;
            renderer.material.color = initial;
            
            while (light.enabled)
            {
                Color newColor = PickRandomPresetColor();
                float transition = 0;
                while (transition < 1)
                {
                    Color materialColor = Color.Lerp(initial, newColor, transition);
                    light.color = materialColor;
                    renderer.material.color = materialColor;
                    transition += 0.03f;
                    yield return new WaitForEndOfFrame();
                }
                ExamplePlugin.Log.LogDebug("Color changed");
                yield return new WaitForSeconds(0.5f);
            }
        }

        private static Color PickRandomPresetColor()
        {
            return from[Random.Range(0, from.Count)];
        }
    }
}