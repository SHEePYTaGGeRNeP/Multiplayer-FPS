using UnityEngine;
using Assets.Scripts;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private string _weaponLayerName = "Weapon";

    [SerializeField]
    private Transform _weaponHolder;

    [SerializeField]
    private PlayerWeapon _primaryWeapon;

    private PlayerWeapon _currentWeapon;
    public PlayerWeapon CurrentWeapon { get { return this._currentWeapon; } }


    private WeaponGraphics _currentGraphics;
    public WeaponGraphics CurrentGraphics { get { return this._currentGraphics; } }

    private void Start()
    {
        this.EquipWeapon(this._primaryWeapon);
    }



    private void EquipWeapon(PlayerWeapon weapon)
    {
        this._currentWeapon = weapon;
        GameObject weaponIns = (GameObject)Instantiate(weapon.Graphics, this._weaponHolder.position, this._weaponHolder.rotation);

        weaponIns.transform.SetParent(this._weaponHolder);
        this._currentGraphics = weaponIns.GetComponent<WeaponGraphics>();
        if (this._currentGraphics == null)
            Debug.LogError("No WeaponGraphics component at the weapon object: " + weaponIns.name);
        if (this.isLocalPlayer)
            Util.SetLayerRecursively(weaponIns, LayerMask.NameToLayer(this._weaponLayerName));


    }


}
