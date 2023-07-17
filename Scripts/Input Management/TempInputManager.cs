using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInputManager : MonoBehaviour
{
    #region singleton
    private static TempInputManager instance;
    public static TempInputManager Instance
    {
        get
        {
            if (instance)
                return instance;
            else
                instance = Create();

            return instance;
        }
    }
    private static TempInputManager Create()
    {
        GameObject newObj = new GameObject("Temp-Input Manager");
        TempInputManager newTIM = newObj.AddComponent<TempInputManager>();

        return newTIM;
    }
    #endregion

    public TempInputs TempInput { get; set; }

    public float OnActionHit { get; set; }//left click/right trigger
    public float OnCancelHit { get; set; }//right click/left trigger

    public bool ButtonWest { get; set; }//(xbox)-> x button/ (PC)-> q key/ (ps4)-> square button
    public bool ButtonNorth { get; set; }//(xbox)-> y button/ (PC)-> e key/ (ps4)-> triangle button
    public bool ButtonEast { get; set; }//(xbox)-> b button/ (PC)-> r key/ (ps4)-> circle button

    public bool Space_Key { get; set; }

    public bool F_Key { get; set; }
    public bool G_Key { get; set; }
    public bool T_Key { get; set; }
    public bool I_Key { get; set; }
    public bool B_Key { get; set; }
    public bool M_Key { get; set; }
    public bool N_Key { get; set; }
    public bool X_Key { get; set; }
    public bool C_Key { get; set; }
    public bool V_Key { get; set; }


    public bool One_Key { get; set; }
    public bool Two_Key { get; set; }
    public bool Three_Key { get; set; }
    public bool Four_Key { get; set; }
    public bool Five_Key { get; set; }


    void Awake()
    {
        TempInput = new TempInputs();
    }
    void OnEnable()
    {
        TempInput.Enable();
    }
    void OnDisable()
    {
        TempInput.Disable();
    }
    void Update()
    {
        OnActionHit = TempInput.general.action.ReadValue<float>();
        OnCancelHit = TempInput.general.cancel.ReadValue<float>();

        ButtonWest = TempInput.general.buttonWest.triggered;
        ButtonNorth = TempInput.general.buttonNorth.triggered;
        ButtonEast = TempInput.general.buttonEast.triggered;

        One_Key = TempInput.general.oneKey.triggered;
        Two_Key = TempInput.general.twoKey.triggered;
        Three_Key = TempInput.general.threeKey.triggered;
        Four_Key = TempInput.general.fourKey.triggered;

        F_Key = TempInput.general.f_Key.triggered;
        G_Key = TempInput.general.g_key.triggered;
        T_Key = TempInput.general.t_key.triggered;
        I_Key = TempInput.general.i_key.triggered;
        B_Key = TempInput.general.b_key.triggered;
        M_Key = TempInput.general.m_key.triggered;
        N_Key = TempInput.general.n_key.triggered;
        X_Key = TempInput.general.x_key.triggered;
        C_Key = TempInput.general.c_key.triggered;
        V_Key = TempInput.general.v_key.triggered;

        Space_Key = TempInput.general.space.triggered;
    }

}
