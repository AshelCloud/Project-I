using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    //모든 몬스터 스크립트의 콜백함수
    public partial class Monster : MonoBehaviour
    {
        public virtual void SetAttack()
        {
            attackID = -1;
            Attack = true;
        }

        public virtual void SetAttack(int id)
        {
            attackID = id;
            Attack = true;
        }

        public virtual int SetRandomAttackID()
        {
            return Random.Range(1, totalAttack + 1);
        }

        public virtual void SetIdle(bool value)
        {
            idle = value;
        }

        public virtual void GetDamaged(float value)
        {
            if (dead) { return; }

            hp -= value;

            if (hp > 0f)
            {
                damaged = true;
            }
            else
            {
                SetDead();
            }
        }

        public virtual void SetChase(bool value)
        {
            chase = value;
        }

        public virtual void AttackTrigger(int triggerIndex)
        {
            if (triggerIndex == -1)
            {
                foreach (AttackTrigger trigger in AttackTriggers)
                {
                    trigger.Collider.enabled = true;
                    trigger.gameObject.SetActive(true);
                }

                return;
            }

            if (triggerIndex == 0)
            {
                foreach (AttackTrigger trigger in AttackTriggers)
                {
                    trigger.Collider.enabled = false;
                    trigger.gameObject.SetActive(false);
                }

                return;
            }

            List<AttackTrigger> At_L = AttackTriggers.FindAll(item => item.index == triggerIndex);

            if (At_L != null)
            {
                foreach (AttackTrigger trigger in At_L)
                {
                    trigger.Collider.enabled = true;
                    trigger.gameObject.SetActive(true);
                }
            }
        }

        private bool LoadToJsonData(int ID)
        {
            Log.Print("Monster: Load JsonData to Monster ID");
            //Json 파싱
            AssetBundle localAssetBundle = AssetBundleContainer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "jsons"));
            if (localAssetBundle == null)
            {
                Log.PrintError("LoadToJosnData: Failed to load AssetBundle!");
                return false;
            }
            TextAsset monsterTable = localAssetBundle.LoadAsset<TextAsset>("MonsterTable");

            var json = JsonManager.LoadJson<Serialization<string, MonsterDataTable>>(monsterTable).ToDictionary();

            //데이터에 몬스터에 해당하는 키가 없으면 return
            if (json.ContainsKey(ID.ToString()) == false)
            {
                Log.PrintError("Failed to Monster Data. ID is null or 0");
                return false;
            }

            dataTable = json[ID.ToString()];
            return true;
        }

        private void DataTableLinking()
        {
            Log.Print("Monster: DataTable Linking");

            objectName = dataTable.ObjectName;
            animatorName = dataTable.AnimatorName;
            offensePower = dataTable.OffensePower;
            defense = dataTable.Defense;
            hp = dataTable.HP;
            speed = dataTable.Speed;
            detectionRange = dataTable.DetectionRange;
            attackRange = dataTable.AttackRange;
            dropBundleID = dataTable.DropBundleID;
        }

        public virtual void SetDead()
        {
            dead = true;

            Anim.SetTrigger(hash_Dead);
        }

        public virtual void SetTurn()
        {
            Vector3 scale = transform.lossyScale;
            scale.x = -scale.x;

            transform.localScale = scale;
        }
    }
}