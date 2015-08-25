using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Scroll : MonoBehaviour
{

    public float ScrollSpeed = 1f;
    public int ItemsInStory = 10;
    public int RepetitionInTicker = 3;

    private List<string> newsItems = new List<string>{
        "Police urge people to disperse into the streets and flee.",
"Reports of Phil Fish growing thicker skin proven false.",
"Looting of local malls, shoppers urged to prioritise acquisition of toilet roll (you'll thank us).",
"Old lady hits monster with handbag, monster levels three square blocks in response.",
"Music played to monster in effort to soothe it, it did not enjoy dubstep.",
"Star quarterback's vintage car launched into the sun.",
"Today's winning lottery numbers: 4 8 15 16 23 42",
"PPI offices flooded with calls, operators request you stop calling them.",
"Mystic who predicted monsters arrival unable to predict being thrown across the city like a discus.",
"City mayor issues cease and desist to monster, monster unphased.",
"House prices along 32nd Avenue trending upwa- nevermind, 32nd Avenue levelled by sneeze.",
"Justin Bieber offers support to survivors, survivors walk willingly towards death.",
"Holidaymakers journeying on budget airline 'so glad' to have needed to stop over in the city.",
"Group of Daenerys Targaryen cosplayers gathered to soothe 'dragon', our thoughts and prayers go out to their families.",
"Work begun on robot version of monster, estimated completion some time after human extinction.",
"Radiation emitted by monster interferring with wifi hotspots, novel writers inconvenienced.",
"Parents admit monsters do exist after one spotted chewing local community centre.",
"Crisis declared officially worse than 'planking' craze.",
"Pot Noodle still in great supply, city expecting famine to be declared momentarily.",
"Song to be written in aid of crisis, Bono offers support, world sighs.",
"Shelters open across the city, taking in anyone, even morris dancers.",
"Resident, Jeff Thompson now regrets not getting home insurance when his wife told him to.",
"Pop Idol auditions thrown into turmoil, people actually turn up.",
"Gordon Ramsey offers his thoughts on situation, mostly expletives.",
"Scotland dredges Loch Ness to find adversary for monster, only highly-agitated haddock discovered.",
"Police release e-fit of monster suspect, or you could look out of your window.",
"Terrifying noises heard across city, reports of Erasure playing local gig.",
"David Icke asked for comment but too aroused by idea creature is heir to British throne.",
"Hopes for toll-bridge restored as monster defecates ticket booth.",
"Outrage as monster uses city monument as toothpick.",
"Monster fined heavily for obstructing loading bay for 12 minutes.",
"Traffic Watch: Just don't.",
"Spike in #FirstWorldProblems hashtag usage.",
"Home of acclaimed movie director Paul W. S. Anderson crushed and I'm not sorry.",
"Creatures breath reported to smell worse than death itself, reminds survivor of old science teacher.",
"Strange smell on city buses now more likely than ever to be urine, scientists say.",
"Citizens urged to avoid 35th Street, reports of scientologists performing recruitment drive.",
"Video camera found in clover field with potential up close footage of creature discovered to just be teary-eyed declarations of love and old dating footage, disappointing.",
"A Pacific rim-shot attempt to erect giant sea-wall to fend off future monsters deemed idiotic.",
"Citizens reminded that no Michael Bay films being presently shot in city. This is real. That is your actual uncle and he is on fire.",
"Newscaster loses bet that this will be a slow news day, had plans for that money.",
"Citizens trapped in local expressionist art exhibit describe scene of destruction, of anger, of raw emotion and feelings.",
"60% of the time Anchorman references work every time.",
"Plans to take off and nuke the site from orbit hailed as brilliant, if not irresponsibly unproductive.",
"Residents urged to only take vital supplies with them, leave that One Direction poster, leave it, let it burn.",
"Apartment complex crushed after monster trips on a convenience store, hilarity briefly ensued before the screams began.",
"Today's Weather: It's raining cars and explosive ordnance. Wear a coat.",
"A tale of misery and destruction, Adam Sandler releases autobiography.",
"Residents evacuate via city sewer system. Nasty. Just nasty.",
"Nearby samba band viciously mauled outside coffee shop.",
    };

    Vector3 startingPos;
    Text text;
    string tickerText;

    // Use this for initialization
    void Start()
    {

        text = GetComponentInChildren<Text>();

        startingPos = text.rectTransform.localPosition;

        string tickerText = getTickerText();

        text.text = tickerText;

    }


    string getTickerText()
    {
        string value = "";
        int c = 0;
        while (c < ItemsInStory)
        {
            value += getStory() + "       -       ";


            c++;
        }
        int r = 0;
        while (r < RepetitionInTicker)
        {
            value += value;

            r++;
        }


        return value;


    }

    string getStory()
    {
        int i = Random.Range(0, newsItems.Count - 1);
        string s = newsItems[i];
        newsItems.Remove(s);

        return s.ToUpper();
    }




    // Update is called once per frame
    void Update()
    {




        Vector3 textPos = text.transform.localPosition;



        if (text.rectTransform.localPosition.x + text.rectTransform.rect.width < startingPos.x - text.rectTransform.rect.width)
        {
            Debug.Log("Resetting Ticker");


            text.rectTransform.localPosition = startingPos;

        }
        else
        {


            textPos.x -= ScrollSpeed;
            text.rectTransform.localPosition = textPos;
        }

    }
}
