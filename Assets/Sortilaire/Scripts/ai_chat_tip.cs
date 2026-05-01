using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ai_chat_tip : MonoBehaviour
{
    public Image img_ai;
    public Image img_tip_play;

    public Text txt_tip_ui;
    public Text txt_tip_home_ui;

    public Sprite[] sp_ai;
    public Sprite[] sp_tip_play;
    public string[] s_tip_home;
    public string[] s_tip_play;

    public string[] s_status_no_block;
    public GameObject obj_guide_play;

    private float timer_next_tip = 0;
    private int index_show = 0;
    private int index_ai = 0;

    private int index_tip_home = 0;

    private bool is_show_tip_play = false;

    void Start()
    {
        if (PlayerPrefs.GetInt("is_show_tip_play", 0) == 0)
            this.is_show_tip_play = true;
        else
            this.is_show_tip_play = false;

        this.update_show_and_hide_tip_play_ui();
        this.update_txt_tip_ui();
        this.update_tip_home();
    }

    void Update()
    {
        if (this.is_show_tip_play)
        {
            this.timer_next_tip += 1f * Time.deltaTime;
            if (this.timer_next_tip > 2f)
            {
                this.next_tip();
                this.next_ai();
                this.timer_next_tip = 0;
            }
        }
    }

    private void update_txt_tip_ui()
    {
        this.txt_tip_ui.text = this.s_tip_play[this.index_show];
    }

    private void next_tip()
    {
        this.index_show++;
        if (this.index_show >= this.s_tip_play.Length) this.index_show = 0;
        this.update_txt_tip_ui();
    }

    private void next_ai()
    {
        this.index_ai++;
        if (this.index_ai >= this.sp_ai.Length) this.index_ai = 0;
        this.update_img_ai_ui();
    }

    private void update_img_ai_ui()
    {
        this.img_ai.sprite = this.sp_ai[this.index_ai];
    }

    public void btn_show_or_hide_guide_play()
    {
        if (this.is_show_tip_play)
        {
            PlayerPrefs.SetInt("is_show_tip_play",1);
            this.is_show_tip_play = false;
        }
        else
        {
            PlayerPrefs.SetInt("is_show_tip_play",0);
            this.is_show_tip_play = true;
        }
        this.update_show_and_hide_tip_play_ui();
    }

    private void update_show_and_hide_tip_play_ui()
    {
        if (this.is_show_tip_play)
            this.obj_guide_play.SetActive(true);
        else
            this.obj_guide_play.SetActive(false);
    }

    public void next_tip_home()
    {
        this.index_tip_home++;
        if (this.index_tip_home >= this.s_tip_home.Length) this.index_tip_home = 0;
        this.update_tip_home();
    }

    public void prev_tip_home()
    {
        this.index_tip_home--;
        if (this.index_tip_home < 0) this.index_tip_home = this.s_tip_home.Length - 1;
        this.update_tip_home();
    }

    private void update_tip_home()
    {
        this.txt_tip_home_ui.text = this.s_tip_home[this.index_tip_home];
        this.img_tip_play.sprite = this.sp_tip_play[this.index_tip_home];
    }

    public void show_fail_block()
    {
        this.timer_next_tip = 0;

    }
}
