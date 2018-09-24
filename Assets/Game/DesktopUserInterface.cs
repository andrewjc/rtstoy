using Game.World.Objects;
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
            if (this.gameMain.GetGameWorld().getPlayerBase() == null) return;

            if (Input.GetMouseButton(0))
            {
                if (selectedUnits.Count > 0)
                {
                    Mineable m;
                    if (isMineableResourceSelected(out m))
                    {
                        // Clicked on a mineable resource...
                        tellSelectedUnitsToGoMine(m);
                        return;
                    }
                }

                CheckUnitSelection();

            }
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
                        if (pu.enabled && pu.playerBase == this.gameMain.GetGameWorld().getPlayerBase())
                        {
                            selectUnit(pu);
                        }
                    }
                    else if (g.GetComponentInParent<PlayerUnit>() != null)
                    {
                        PlayerUnit pu = g.GetComponentInParent<PlayerUnit>();
                        if (pu.enabled && pu.playerBase == this.gameMain.GetGameWorld().getPlayerBase())
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

        private void tellSelectedUnitsToGoMine(Mineable mineable)
        {
            foreach (PlayerUnit u in selectedUnits)
            {
                if (u.GetComponent<Miner>() == null) return;

                u.ClearTaskList();

                if ((mineable is MineableCrystal))
                    u.AddTask(u.gameObject.AddComponent<CrystalMinerTask>().SetMaxResource(9999).SetSelected((MineableCrystal)mineable));
            }
        }

        private bool isMineableResourceSelected(out Mineable mineable)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var resourcesLayerMask = LayerMask.GetMask("Crystals");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, resourcesLayerMask))
            {
                Transform objectHit = hit.transform;

                if (hit.collider != null && hit.collider.gameObject != null)
                {
                    GameObject g = hit.collider.gameObject;
                    if (g.GetComponent<Mineable>() != null)
                    {
                        mineable = g.GetComponent<Mineable>();
                        return true;
                    }
                }
            }

            mineable = null;
            return false;
        }
    }
}