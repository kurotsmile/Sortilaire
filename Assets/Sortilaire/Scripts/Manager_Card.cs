using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Card_Type
{
    Spades,Diamond,Clubs,Hearts,None
}

public class Manager_Card : MonoBehaviour
{
    public Transform tr_play;
    public Sprite[] sp_card;
    public Card_Type[] card_type_index;
    public int[] card_number_index;

    public GameObject card_obj_prefab;
    public card_tray card_tray_open;
    public card_tray card_tray_close;
    public card_tray[] card_tray_play;

    public Text txt_count_move;
    public Text txt_count_timer;

    private int[] card_type = new int[52];
    private int count_move = 0;

    private float time_second;
  

    public void load()
    {
        this.reset();
    }

    private void Update()
    {
        this.time_second += 1 * Time.deltaTime;
        TimeSpan t = TimeSpan.FromSeconds(time_second);
        string s_t = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
        this.txt_count_timer.text = s_t;
    }

    public void create_card_in_tray_main()
    {
        for (int i = 0; i < 52; i++)
        {
            this.card_type[i] = i;
        }

        this.Shuffle(this.card_type);

        for (int i = 0; i < this.card_type.Length; i++)
        {
            
            this.card_tray_close.add_card(this.create_card(this.card_type[i]));
        }

        for (int i = 0; i < this.card_tray_play.Length; i++)
        {
            this.card_tray_play[i].name = "Card_tray_" + i;
            if(i==12)
                this.card_tray_play[i].index_tray = -1;
            else
                this.card_tray_play[i].index_tray = i;

            if (this.card_tray_play[i].is_tray_docking)
            {
                for (int y = 0; y < i + 1; y++)
                {
                    this.card_tray_play[i].add_card(this.card_tray_close.get_card_random());
                }
                this.card_tray_play[i].close_in_tray_no_first();
            }
            
        }
    }

    private GameObject create_card(int type)
    {
        GameObject obj_card = Instantiate(this.card_obj_prefab);
        obj_card.GetComponent<card_obj>().img_card.sprite = this.sp_card[type];
        obj_card.GetComponent<card_obj>().type = type;
        obj_card.GetComponent<card_obj>().card_type = this.card_type_index[type];
        obj_card.GetComponent<card_obj>().card_number = this.card_number_index[type];
        obj_card.GetComponent<card_obj>().transform_play = this.tr_play;
        return obj_card;
    }

    void Shuffle(int[] array)
    {
        int p = array.Length;
        for (int n = p - 1; n > 0; n--)
        {
            int r = UnityEngine.Random.Range(1, n);
            int t = array[r];
            array[r] = array[n];
            array[n] = t;
        }
    }

    public void unclick_all_card()
    {
        for (int i = 0; i < this.card_tray_play.Length; i++)
        {
            this.card_tray_play[i].unclick_all_card();
        }
    }

    public void remove_carde_in_tray(int index_tray)
    {
        this.card_tray_play[index_tray].remove_card_last();
    }

    public void open_new_card()
    {
        if (this.card_tray_close.get_count_card() > 0)
        {
            this.card_tray_open.add_card(this.card_tray_close.get_card_last_and_reamove());
            this.card_tray_open.get_card_last().GetComponent<card_obj>().open();
        }
    }

    public void get_group_card(card_obj card_check)
    {
        if(card_check.index_tray<=7) this.card_tray_play[card_check.index_tray].check_group(card_check);
    }

    public void reset()
    {
        this.time_second = 0;
        this.count_move = 0;
        this.update_txt_move_ui();
        this.clear_all_tray_card();
        this.create_card_in_tray_main();
        this.txt_count_timer.text = "0";
    }

    private void clear_all_tray_card()
    {
        for (int i = 0; i < this.card_tray_play.Length; i++)
        {
            this.card_tray_play[i].clear();
        }
    }

    public void check_win_game()
    {
        int coun_full = 0;
        for(int i = 0; i < this.card_tray_play.Length; i++)
        {
            if (this.card_tray_play[i].is_tray_done) {
                coun_full += this.card_tray_play[i].get_count_card();
            }
        }

        if (coun_full >= 52)
        {
            this.GetComponent<Game_handle>().show_game_win();
        }
    }

    private void update_txt_move_ui()
    {
        this.txt_count_move.text = this.count_move.ToString();
    }

    public void add_count_move()
    {
        this.count_move++;
        this.update_txt_move_ui();
    }

    public void check_in_tray_true(card_obj card_check)
    {
        for (int i = 0; i < this.card_tray_play.Length; i++)
        {
            if (i != card_check.index_tray)
            {
                this.card_tray_play[i].check_true_in_tray(card_check);
            }
        }
    }

    public void un_check_all_in_tray()
    {
        for (int i = 0; i < this.card_tray_play.Length; i++)
        {
            if(this.card_tray_play[i].is_tray_docking|| this.card_tray_play[i].is_tray_done) this.card_tray_play[i].reset_check();
        }
    }
}
