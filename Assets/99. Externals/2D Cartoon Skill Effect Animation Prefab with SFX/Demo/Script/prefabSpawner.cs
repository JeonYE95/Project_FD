using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Art_Controller
{


    public class prefabSpawner : MonoBehaviour
    {

        public List<GameObject> prefabs;
        public SpriteRenderer character;

        public void SpawnPrefab(int index)
        {
            StartCoroutine(PrefabAnimation(index));

        }

        public IEnumerator CharacterHit()
        {
            float transparency = character.color.a;
            for (int i = 0; i < 1; i++)
            {
                while (transparency > 0.3)
                {
                    transparency -= 0.1f;
                    character.color = new Color(1, 1, 1, transparency);
                    yield return new WaitForSeconds(0.02f);
                }
                while (transparency < 1)
                {
                    transparency += 0.1f;
                    character.color = new Color(1, 1, 1, transparency);
                    yield return new WaitForSeconds(0.02f);
                }
            }

        }

        public IEnumerator PrefabAnimation(int index)
        {
            GameObject prefab = Instantiate(prefabs[index], new Vector3(0, 0, 0), Quaternion.identity);
            SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
            float transparency = spriteRenderer.color.a;
            Animator animator = prefab.GetComponent<Animator>();
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8);
            float duration = animator.GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(CharacterHit());
            while (transparency > 0)
            {
                transparency -= 0.1f;
                spriteRenderer.color = new Color(1, 1, 1, transparency);
                yield return new WaitForSeconds(duration * 0.05f);
            }
            Destroy(prefab);

        }
    }

}
