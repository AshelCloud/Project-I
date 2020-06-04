using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerTest
    {
        private Vector3 playerPosition = new Vector3(0, 10);
        private Vector3 groundPosition = new Vector3(0, 0);

        [UnityTest]
        public IEnumerator PlayerGroundedCheck()
        {
            Player player = Object.Instantiate(Resources.Load<Player>("Prefabs/Player"), playerPosition, Quaternion.identity);

            GameObject ground = new GameObject();

            BoxCollider2D collider = ground.AddComponent<BoxCollider2D>();

            if (collider != null)
            {
                collider.isTrigger = false;
                collider.usedByEffector = false;
                collider.usedByComposite = false;
                collider.autoTiling = false;
                collider.offset = new Vector2(0f, 0f);
                collider.size = new Vector2(20f, 1f);
                collider.edgeRadius = 0f;
                collider.tag = "Floor";
            }

            var go = Object.Instantiate(ground, groundPosition, Quaternion.identity);

            Assert.IsFalse(player.isGrounded);
            Debug.Log("최초 좌표" + player.transform.position.y.ToString());
            yield return new WaitForSeconds(3f);


            Debug.Log("낙하 좌표" + player.transform.position.y.ToString());
            Assert.IsTrue(player.isGrounded);

        }
    }
}
