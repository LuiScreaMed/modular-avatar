﻿using System.Collections.Generic;
using System.Linq;
using nadena.dev.modular_avatar.core.menu;
using UnityEngine;
using UnityEngine.Serialization;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace nadena.dev.modular_avatar.core
{
    public enum SubmenuSource
    {
        MenuAsset,
        Children,
    }

    [AddComponentMenu("Modular Avatar/MA Menu Item")]
    public class ModularAvatarMenuItem : MenuSourceComponent
    {
        public VRCExpressionsMenu.Control Control;
        public SubmenuSource MenuSource;

        public GameObject menuSource_otherObjectChildren;

        [FormerlySerializedAs("toggleGroup")] public ControlGroup controlGroup;
        public bool isDefault;

        protected override void OnValidate()
        {
            base.OnValidate();

            if (Control == null)
            {
                Control = new VRCExpressionsMenu.Control();
            }
        }

        public override void Visit(NodeContext context)
        {
            if (Control == null)
            {
                Control = new VRCExpressionsMenu.Control();
            }

            var cloned = new VirtualControl(Control);
            cloned.subMenu = null;
            cloned.name = gameObject.name;

            if (cloned.type == VRCExpressionsMenu.Control.ControlType.SubMenu)
            {
                switch (this.MenuSource)
                {
                    case SubmenuSource.MenuAsset:
                        cloned.SubmenuNode = context.NodeFor(this.Control.subMenu);
                        break;
                    case SubmenuSource.Children:
                    {
                        var root = this.menuSource_otherObjectChildren != null
                            ? this.menuSource_otherObjectChildren
                            : this.gameObject;

                        cloned.SubmenuNode = context.NodeFor(new MenuNodesUnder(root));
                        break;
                    }
                }
            }

            context.PushControl(cloned);
        }
    }
}