using gameServer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameServer.Common
{
    public class StructPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Info_ Info { get; set; }


        public StructPlayer()
        {
            Id = -1;
        }

        public Dictionary<byte, object> SerializationPlayerToDict()
        {
            Dictionary<byte, object> dict = new Dictionary<byte, object>();
            Dictionary<byte, object> template = new Dictionary<byte, object>();
            Dictionary<byte, object> body = new Dictionary<byte, object>();
            Dictionary<byte, object> motorElement = new Dictionary<byte, object>();
            Dictionary<byte, object> weapons = new Dictionary<byte, object>();
            Dictionary<byte, object> weaponsBack = new Dictionary<byte, object>();
            Dictionary<byte, object> weaponsFront = new Dictionary<byte, object>();
            Dictionary<byte, object> weaponsLeft = new Dictionary<byte, object>();
            Dictionary<byte, object> weaponsRight = new Dictionary<byte, object>();
            Dictionary<byte, object> weaponsTop = new Dictionary<byte, object>();

            template.Add((byte)PlayerTemplateCode.PlayerBody, body);
            template.Add((byte)PlayerTemplateCode.MotorElement, motorElement);
            template.Add((byte)PlayerTemplateCode.Weapons, weapons);

            body.Add((byte)PlayerTemplateCode.Mesh, Info.Template.Body.Mesh);
            body.Add((byte)PlayerTemplateCode.Skin, Info.Template.Body.Skin);

            motorElement.Add((byte)PlayerTemplateCode.Mesh, Info.Template.MotorElement.Mesh);
            motorElement.Add((byte)PlayerTemplateCode.Skin, Info.Template.MotorElement.Skin);

            weapons.Add((byte)PlayerTemplateCode.WeaponsBack, weaponsBack);
            weapons.Add((byte)PlayerTemplateCode.WeaponsFront, weaponsFront);
            weapons.Add((byte)PlayerTemplateCode.WeaponsLeft, weaponsLeft);
            weapons.Add((byte)PlayerTemplateCode.WeaponsRight, weaponsRight);
            weapons.Add((byte)PlayerTemplateCode.WeaponsTop, weaponsTop);

            weaponsBack.Add((byte)PlayerTemplateCode.Mesh, Info.Template.Weapons.WeaponsBack.Mesh);
            weaponsBack.Add((byte)PlayerTemplateCode.Skin, Info.Template.Weapons.WeaponsBack.Skin);

            weaponsFront.Add((byte)PlayerTemplateCode.Mesh, Info.Template.Weapons.WeaponsFront.Mesh);
            weaponsFront.Add((byte)PlayerTemplateCode.Skin, Info.Template.Weapons.WeaponsFront.Skin);

            weaponsRight.Add((byte)PlayerTemplateCode.Mesh, Info.Template.Weapons.WeaponsRight.Mesh);
            weaponsRight.Add((byte)PlayerTemplateCode.Skin, Info.Template.Weapons.WeaponsRight.Skin);

            weaponsLeft.Add((byte)PlayerTemplateCode.Mesh, Info.Template.Weapons.WeaponsLeft.Mesh);
            weaponsLeft.Add((byte)PlayerTemplateCode.Skin, Info.Template.Weapons.WeaponsLeft.Skin);

            weaponsTop.Add((byte)PlayerTemplateCode.Mesh, Info.Template.Weapons.WeaponsTop.Mesh);
            weaponsTop.Add((byte)PlayerTemplateCode.Skin, Info.Template.Weapons.WeaponsTop.Skin);

            dict.Add((byte)ParameterCode.Id, Id);
            dict.Add((byte)ParameterCode.CharactedName, Name);
            dict.Add((byte)ParameterCode.PlayerInfo, template);

            return dict;
        }

        public void DeserializationPlayerFromDict(Dictionary<byte, object> dict)
        {
            Id = (int)dict[(byte)ParameterCode.Id];
            Name = (string)dict[(byte)ParameterCode.CharactedName];

            Template template = new Template();
            Info_ info = new Info_();
            Body body = new Body();
            MotorElement motorElement = new MotorElement();
            Weapons weapons = new Weapons();
            WeaponsBack weaponsBack = new WeaponsBack();
            WeaponsFront weaponsFront = new WeaponsFront();
            WeaponsLeft weaponsLeft = new WeaponsLeft();
            WeaponsRight weaponsRight = new WeaponsRight();
            WeaponsTop weaponsTop = new WeaponsTop();

            Dictionary<byte, object> dictInfo = (Dictionary<byte, object>)dict[(byte)ParameterCode.PlayerInfo];
            Dictionary<byte, object> dictBody = (Dictionary<byte, object>)dictInfo[(byte)PlayerTemplateCode.PlayerBody];
            Dictionary<byte, object> dictMotElem = (Dictionary<byte, object>)dictInfo[(byte)PlayerTemplateCode.MotorElement];
            Dictionary<byte, object> dictWeapon = (Dictionary<byte, object>)dictInfo[(byte)PlayerTemplateCode.Weapons];

            Dictionary<byte, object> dictWeaponT = (Dictionary<byte, object>)dictWeapon[(byte)PlayerTemplateCode.WeaponsTop];
            Dictionary<byte, object> dictWeaponB = (Dictionary<byte, object>)dictWeapon[(byte)PlayerTemplateCode.WeaponsBack];
            Dictionary<byte, object> dictWeaponF = (Dictionary<byte, object>)dictWeapon[(byte)PlayerTemplateCode.WeaponsFront];
            Dictionary<byte, object> dictWeaponL = (Dictionary<byte, object>)dictWeapon[(byte)PlayerTemplateCode.WeaponsLeft];
            Dictionary<byte, object> dictWeaponR = (Dictionary<byte, object>)dictWeapon[(byte)PlayerTemplateCode.WeaponsRight];

            body.Mesh = (byte)dictBody[(byte)PlayerTemplateCode.Mesh];
            body.Skin = (byte)dictBody[(byte)PlayerTemplateCode.Skin];

            motorElement.Mesh = (byte)dictMotElem[(byte)PlayerTemplateCode.Mesh];
            motorElement.Skin = (byte)dictMotElem[(byte)PlayerTemplateCode.Skin];

            weaponsBack.Mesh = (byte)dictWeaponB[(byte)PlayerTemplateCode.Mesh];
            weaponsBack.Skin = (byte)dictWeaponB[(byte)PlayerTemplateCode.Skin];

            weaponsFront.Mesh = (byte)dictWeaponF[(byte)PlayerTemplateCode.Mesh];
            weaponsFront.Skin = (byte)dictWeaponF[(byte)PlayerTemplateCode.Skin];

            weaponsLeft.Mesh = (byte)dictWeaponL[(byte)PlayerTemplateCode.Mesh];
            weaponsLeft.Skin = (byte)dictWeaponL[(byte)PlayerTemplateCode.Skin];

            weaponsRight.Mesh = (byte)dictWeaponR[(byte)PlayerTemplateCode.Mesh];
            weaponsRight.Skin = (byte)dictWeaponR[(byte)PlayerTemplateCode.Skin];

            weaponsTop.Mesh = (byte)dictWeaponT[(byte)PlayerTemplateCode.Mesh];
            weaponsTop.Skin = (byte)dictWeaponT[(byte)PlayerTemplateCode.Skin];

            weapons.WeaponsBack = weaponsBack;
            weapons.WeaponsFront = weaponsFront;
            weapons.WeaponsLeft = weaponsLeft;
            weapons.WeaponsRight = weaponsRight;
            weapons.WeaponsTop = weaponsTop;

            template.Body = body;
            template.MotorElement = motorElement;
            template.Weapons = weapons;
            info.Template = template;
            Info = info;
        }

        public static Info_ getNUllPlayerTemplate()
        {
            Template template = new Template();
            Info_ info = new Info_();
            Body body = new Body();
            MotorElement motorElement = new MotorElement();
            Weapons weapons = new Weapons();
            WeaponsBack weaponsBack = new WeaponsBack();
            WeaponsFront weaponsFront = new WeaponsFront();
            WeaponsLeft weaponsLeft = new WeaponsLeft();
            WeaponsRight weaponsRight = new WeaponsRight();
            WeaponsTop weaponsTop = new WeaponsTop();

            body.Mesh = (byte)PlayerTemlateValueCode.Default;
            body.Skin = (byte)PlayerTemlateValueCode.Default;
            motorElement.Mesh = (byte)PlayerTemlateValueCode.Default;
            motorElement.Skin = (byte)PlayerTemlateValueCode.Default;

            weaponsBack.Mesh = (byte)PlayerTemlateValueCode.Default;
            weaponsBack.Skin = (byte)PlayerTemlateValueCode.Default;
            
            weaponsFront.Mesh = (byte)PlayerTemlateValueCode.Default;
            weaponsFront.Skin = (byte)PlayerTemlateValueCode.Default;
            
            weaponsLeft.Mesh = (byte)PlayerTemlateValueCode.Default;
            weaponsLeft.Skin = (byte)PlayerTemlateValueCode.Default;
            
            weaponsRight.Mesh = (byte)PlayerTemlateValueCode.Default;
            weaponsRight.Skin = (byte)PlayerTemlateValueCode.Default;
            
            weaponsTop.Mesh = (byte)PlayerTemlateValueCode.Default;
            weaponsTop.Skin = (byte)PlayerTemlateValueCode.Default;

            weapons.WeaponsBack = weaponsBack;
            weapons.WeaponsFront = weaponsFront;
            weapons.WeaponsLeft = weaponsLeft;
            weapons.WeaponsRight = weaponsRight;
            weapons.WeaponsTop = weaponsTop;

            template.Body = body;
            template.MotorElement = motorElement;
            template.Weapons = weapons;
            info.Template = template;
            return info;
        }
    }

    #region Serialize
    public class Info_
    {
        public Template Template { get; set; }
    }

    public class Template
    {
        public Body Body { get; set; }
        public MotorElement MotorElement { get; set; }
        public Weapons Weapons { get; set; }
    }

    public class Body
    {
        public byte Mesh { get; set; }
        public byte Skin { get; set; }
    }

    public class MotorElement
    {
        public byte Mesh { get; set; }
        public byte Skin { get; set; }

    }
    public class Weapons
    {
        public WeaponsTop WeaponsTop { get; set; }
        public WeaponsBack WeaponsBack { get; set; }
        public WeaponsFront WeaponsFront { get; set; }
        public WeaponsRight WeaponsRight { get; set; }
        public WeaponsLeft WeaponsLeft { get; set; }
    }

    public class WeaponsTop
    {
        public byte Mesh { get; set; }
        public byte Skin { get; set; }
    }
    public class WeaponsBack
    {
        public byte Mesh { get; set; }
        public byte Skin { get; set; }
    }
    public class WeaponsFront
    {
        public byte Mesh { get; set; }
        public byte Skin { get; set; }
    }
    public class WeaponsRight
    {
        public byte Mesh { get; set; }
        public byte Skin { get; set; }
    }
    public class WeaponsLeft
    {
        public byte Mesh { get; set; }
        public byte Skin { get; set; }
    }
    #endregion

}
