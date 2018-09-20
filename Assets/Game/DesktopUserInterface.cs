using Game.World.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    internal class DesktopUserInterface
    {
        private GameMain gameMain;
        private LinkedList<PlayerUnit> selectedUnits;

        public DesktopUserInterface(GameMain gameMain)
        {
            this.gameMain = gameMain;
        }

        public void Awake()
        {
            this.selectedUnits = new LinkedList<PlayerUnit>();
        }


        internal void Update()
        {
            if (this.gameMain.getGameWorld().getPlayerBase() == null) return;

            if(Input.GetMouseButton(0))
                CheckUnitSelection();
        }

        private void CheckUnitSelection()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                deselectAllUnits();
            }
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var buildingsLayerMask = LayerMask.GetMask("Units", "Buildings");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingsLayerMask))
            {
                Transform objectHit = hit.transform;

                if (hit.collider != null && hit.collider.gameObject != null)
                {
                    GameObject g = hit.collider.gameObject;

                    if (g.GetComponent<PlayerUnit>() != null)
                    {
                        PlayerUnit pu = g.GetComponent<PlayerUnit>();
                        if (pu.enabled && pu.playerBase == this.gameMain.getGameWorld().getPlayerBase())
                        {
                            selectUnit(pu);
                        }
                    }
                    else if (g.GetComponentInParent<PlayerUnit>() != null)
                    {
                        PlayerUnit pu = g.GetComponentInParent<PlayerUnit>();
                        if (pu.enabled && pu.playerBase == this.gameMain.getGameWorld().getPlayerBase())
                        {
                            selectUnit(pu);
                        }
                    }
                }
            }
        }

        private void selectUnit(PlayerUnit pu)
        {
            selectedUnits.AddLast(pu);
            pu.OnSelected();
        }

        private void deselectAllUnits()
        {
            foreach (PlayerUnit pu in selectedUnits)
            {
                pu.Deselect();
            }
            selectedUnits.Clear();
        }
    }
}