﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System;
using TMPro;
using UnityEngine.EventSystems;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour {

        Health health;

        enum CursorType {
            None, Movement, Combat, UI
        }

        [System.Serializable]
        struct CursorMapping {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        private void Awake() {
            GameObject.FindWithTag("Respawn").GetComponent<TextMeshProUGUI>().enabled = false;
            health = GetComponent<Health>();
        }

        private void Update() {
            if (InteractWithUI())
                return;
            if (health.IsDead()) {
                GameObject.FindWithTag("Respawn").GetComponent<TextMeshProUGUI>().enabled = true;
                //SetCursor(CursorType.None);
                return;
            }
            if (InteractWithCombat())
                return;
            if(InteractWithMovement())
                return;
            //SetCursor(CursorType.None);
        }

        private bool InteractWithUI() {
            
            if (EventSystem.current.IsPointerOverGameObject()) {
                //SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits) {
                CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();
                if (target == null)
                    continue;
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                    continue;
                        
                if (Input.GetMouseButton(0)) {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }

                //SetCursor(CursorType.Combat);
                return true;
        
                
            }
            return false;
        }


        private bool InteractWithMovement() {
 
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            
            if (hasHit) {
                if (Input.GetMouseButton(0)) {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                //SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType type) {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type) {
            foreach(CursorMapping mapping in cursorMappings) {
                if(mapping.type == type) {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
