using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_handle : MonoBehaviour
{
    [Header("Object Game")]
    public Carrot.Carrot carrot;
    public Manager_Card manager_card;
    public ai_chat_tip ai_chat;

    [Header("Panel Game")]
    public GameObject panel_home;
    public GameObject panel_play;
    public GameObject panel_win;
    public GameObject panel_tip;
    public Text txt_count_win_home;

    public AudioSource[] sound;
    private int count_win = 0;

    void Start()
    {
        this.carrot.Load_Carrot(this.check_exit_app);
        this.manager_card.reset();

        this.panel_home.SetActive(true);
        this.panel_win.SetActive(false);
        this.panel_play.SetActive(false);
        this.panel_tip.SetActive(false);

        this.count_win = PlayerPrefs.GetInt("count_win",0);
        this.update_count_win_txt_ui();

        if (this.carrot.get_status_sound()) this.carrot.game.load_bk_music(this.sound[0]);
    }

    private void check_exit_app()
    {
        if (this.panel_play.activeInHierarchy)
        {
            this.btn_back_home();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_win.activeInHierarchy)
        {
            this.btn_back_home();
            this.carrot.set_no_check_exit_app();
        }else if (this.panel_tip.activeInHierarchy)
        {
            this.btn_close_tip();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void btn_start_game()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.play_sound_click();
        this.panel_home.SetActive(false);
        this.panel_play.SetActive(true);
    }

    public void btn_back_home()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.play_sound_click();
        this.panel_home.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_win.SetActive(false);
    }

    public void btn_open_new_card()
    {
        this.carrot.play_sound_click();
        this.manager_card.open_new_card();
    }

    public void btn_reset_game()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.play_sound(3);
        this.manager_card.reset();
        this.panel_play.SetActive(true);
        this.panel_home.SetActive(false);
        this.panel_win.SetActive(false);
    }

    public void play_sound(int index_sound)
    {
        if (this.carrot.get_status_sound()) this.sound[index_sound].Play();
    }

    public void show_game_win()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.play_sound(4);
        this.panel_win.SetActive(true);


        this.count_win++;
        this.update_count_win_txt_ui();
        PlayerPrefs.SetInt("count_win",this.count_win);
        this.carrot.game.update_scores_player(this.count_win);
    }

    public void btn_rate()
    {
        this.carrot.show_rate();
    }

    public void btn_user()
    {
        this.carrot.user.show_login();
    }

    public void btn_share()
    {
        this.carrot.show_share();
    }

    public void btn_top_player()
    {
        this.carrot.game.Show_List_Top_player();
    }

    public void btn_show_app_other()
    {
        this.carrot.show_list_carrot_app();
    }

    public void btn_setting()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.ads.Destroy_Banner_Ad();
        Carrot.Carrot_Box box_setting=this.carrot.Create_Setting();
        box_setting.set_act_before_closing(act_after_close_setting);
    }

    private void act_after_close_setting(List<string> list_change)
    {
        foreach(string s in list_change)
        {
            if (s == "list_bk_music") this.carrot.game.load_bk_music(this.sound[0]);
        }

        if (this.carrot.get_status_sound())
            this.sound[0].Play();
        else
            this.sound[0].Stop();
        this.carrot.ads.create_banner_ads();
    }

    private void update_count_win_txt_ui()
    {
        this.txt_count_win_home.text = this.count_win.ToString();
    }

    public void show_guide()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.play_sound_click();
        this.ai_chat.btn_show_or_hide_guide_play();
    }

    public void btn_show_tip()
    {
        this.carrot.play_sound_click();
        this.panel_tip.SetActive(true);
    }

    public void btn_close_tip()
    {
        this.carrot.play_sound_click();
        this.panel_tip.SetActive(false);
    }

    public void btn_next_tip_home()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.play_sound_click();
        this.ai_chat.next_tip_home();
    }

    public void btn_prev_tip_home()
    {
        this.carrot.play_sound_click();
        this.ai_chat.prev_tip_home();
    }
}
