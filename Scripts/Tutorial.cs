using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Terminal terminal;
    public Image terminalButtonImage, profileimage;
    public GameObject TerminalPanel, MainCamera;
    public AudioSource vfxsource;
    public AudioClip notificationClip, successClip, failClip;
    public InputField commandField;
    public TMPro.TMP_InputField docviewer;
    public Image imageviewer;
    public Button Profile, ImageViewer, DocViewer, AudioPlayer, Settings, Decrypt;
    public ImageDocument dekryptFile;
    // Start is called before the first frame update
    void Start()
    {
        if(!terminal.notutorial)
        StartCoroutine(tutorialRing());
        else
        {
            commandField.interactable = true;  
            Profile.interactable = true;
            ImageViewer.interactable = true;
            DocViewer.interactable = true;
            AudioPlayer.interactable = true;
            Settings.interactable = true;
            Decrypt.gameObject.SetActive(true);
            Decrypt.interactable = true;
        }
        //StartCoroutine(finalRing());
    }

    static bool ContainsBadWord(string input)
    {
        // List of bad words
        string[] badWords = { "2 girls 1 cup", "2g1c", "4r5e", "5h1t", "5hit", "a55", "a_s_s", "acrotomophilia", "alabama hot pocket", "alaskan pipeline", "anal", "anilingus", "anus", "apeshit", "ar5e", "arrse", "arse", "arsehole", "ass", "ass-fucker", "ass-hat", "ass-pirate", "assbag", "assbandit", "assbanger", "assbite", "assclown", "asscock", "asscracker", "asses", "assface", "assfucker", "assfukka", "assgoblin", "asshat", "asshead", "asshole", "assholes", "asshopper", "assjacker", "asslick", "asslicker", "assmonkey", "assmunch", "assmuncher", "asspirate", "assshole", "asssucker", "asswad", "asswhole", "asswipe", "auto erotic", "autoerotic", "b!tch", "b00bs", "b17ch", "b1tch", "babeland", "baby batter", "baby juice", "ball gag", "ball gravy", "ball kicking", "ball licking", "ball sack", "ball sucking", "ballbag", "balls", "ballsack", "bampot", "bangbros", "bareback", "barely legal", "barenaked", "bastard", "bastardo", "bastinado", "bbw", "bdsm", "beaner", "beaners", "beastial", "beastiality", "beastility", "beaver cleaver", "beaver lips", "bellend", "bestial", "bestiality", "bi+ch", "biatch", "big black", "big breasts", "big knockers", "big tits", "bimbos", "birdlock", "bitch", "bitcher", "bitchers", "bitches", "bitchin", "bitching", "black cock", "blonde action", "blonde on blonde action", "bloody", "blow job", "blow your load", "blowjob", "blowjobs", "blue waffle", "blumpkin", "boiolas", "bollock", "bollocks", "bollok", "bollox", "bondage", "boner", "boob", "boobie", "boobs", "booobs", "boooobs", "booooobs", "booooooobs", "booty call", "breasts", "brown showers", "brunette action", "buceta", "bugger", "bukkake", "bulldyke", "bullet vibe", "bullshit", "bum", "bung hole", "bunghole", "bunny fucker", "busty", "butt", "butt-pirate", "buttcheeks", "butthole", "buttmunch", "buttplug", "c0ck", "c0cksucker", "camel toe", "camgirl", "camslut", "camwhore", "carpet muncher", "carpetmuncher", "cawk", "chinc", "chink", "choad", "chocolate rosebuds", "chode", "cipa", "circlejerk", "cl1t", "cleveland steamer", "clit", "clitface", "clitoris", "clits", "clover clamps", "clusterfuck", "cnut", "cock", "cock-sucker", "cockbite", "cockburger", "cockface", "cockhead", "cockjockey", "cockknoker", "cockmaster", "cockmongler", "cockmongruel", "cockmonkey", "cockmunch", "cockmuncher", "cocknose", "cocknugget", "cocks", "cockshit", "cocksmith", "cocksmoker", "cocksuck", "cocksuck ", "cocksucked", "cocksucked ", "cocksucker", "cocksucking", "cocksucks ", "cocksuka", "cocksukka", "cok", "cokmuncher", "coksucka", "coochie", "coochy", "coon", "coons", "cooter", "coprolagnia", "coprophilia", "cornhole", "cox", "crap", "creampie", "cum", "cumbubble", "cumdumpster", "cumguzzler", "cumjockey", "cummer", "cumming", "cums", "cumshot", "cumslut", "cumtart", "cunilingus", "cunillingus", "cunnie", "cunnilingus", "cunt", "cuntface", "cunthole", "cuntlick", "cuntlick ", "cuntlicker", "cuntlicker ", "cuntlicking", "cuntlicking ", "cuntrag", "cunts", "cyalis", "cyberfuc", "cyberfuck ", "cyberfucked ", "cyberfucker", "cyberfuckers", "cyberfucking ", "d1ck", "dammit", "damn", "darkie", "date rape", "daterape", "deep throat", "deepthroat", "dendrophilia", "dick", "dickbag", "dickbeater", "dickface", "dickhead", "dickhole", "dickjuice", "dickmilk", "dickmonger", "dickslap", "dicksucker", "dickwad", "dickweasel", "dickweed", "dickwod", "dike", "dildo", "dildos", "dingleberries", "dingleberry", "dink", "dinks", "dipshit", "dirsa", "dirty pillows", "dirty sanchez", "dlck", "dog style", "dog-fucker", "doggie style", "doggiestyle", "doggin", "dogging", "doggy style", "doggystyle", "dolcett", "domination", "dominatrix", "dommes", "donkey punch", "donkeyribber", "doochbag", "dookie", "doosh", "double dong", "double penetration", "douche", "douchebag", "dp action", "dry hump", "duche", "dumbshit", "dumshit", "dvda", "dyke", "eat my ass", "ecchi", "ejaculate", "ejaculated", "ejaculates ", "ejaculating ", "ejaculatings", "ejaculation", "ejakulate", "erotic", "erotism", "escort", "eunuch", "f u c k", "f u c k e r", "f4nny", "f_u_c_k", "fag", "fagbag", "fagg", "fagging", "faggit", "faggitt", "faggot", "faggs", "fagot", "fagots", "fags", "fagtard", "fanny", "fannyflaps", "fannyfucker", "fanyy", "fart", "farted", "farting", "farty", "fatass", "fcuk", "fcuker", "fcuking", "fecal", "feck", "fecker", "felatio", "felch", "felching", "fellate", "fellatio", "feltch", "female squirting", "femdom", "figging", "fingerbang", "fingerfuck ", "fingerfucked ", "fingerfucker ", "fingerfuckers", "fingerfucking ", "fingerfucks ", "fingering", "fistfuck", "fistfucked ", "fistfucker ", "fistfuckers ", "fistfucking ", "fistfuckings ", "fistfucks ", "fisting", "flamer", "flange", "fook", "fooker", "foot fetish", "footjob", "frotting", "fuck", "fuck buttons", "fucka", "fucked", "fucker", "fuckers", "fuckhead", "fuckheads", "fuckin", "fucking", "fuckings", "fuckingshitmotherfucker", "fuckme ", "fucks", "fucktards", "fuckwhit", "fuckwit", "fudge packer", "fudgepacker", "fuk", "fuker", "fukker", "fukkin", "fuks", "fukwhit", "fukwit", "futanari", "fux", "fux0r", "g-spot", "gang bang", "gangbang", "gangbanged", "gangbanged ", "gangbangs ", "gay sex", "gayass", "gaybob", "gaydo", "gaylord", "gaysex", "gaytard", "gaywad", "genitals", "giant cock", "girl on", "girl on top", "girls gone wild", "goatcx", "goatse", "god damn", "god-dam", "god-damned", "goddamn", "goddamned", "gokkun", "golden shower", "goo girl", "gooch", "goodpoop", "gook", "goregasm", "gringo", "grope", "group sex", "guido", "guro", "hand job", "handjob", "hard core", "hardcore", "hardcoresex ", "heeb", "hell", "hentai", "heshe", "ho", "hoar", "hoare", "hoe", "hoer", "homo", "homoerotic", "honkey", "honky", "hooker", "hore", "horniest", "horny", "hot carl", "hot chick", "hotsex", "how to kill", "how to murder", "huge fat", "humping", "incest", "intercourse", "jack off", "jack-off ", "jackass", "jackoff", "jail bait", "jailbait", "jap", "jelly donut", "jerk off", "jerk-off ", "jigaboo", "jiggaboo", "jiggerboo", "jism", "jiz", "jiz ", "jizm", "jizm ", "jizz", "juggs", "kawk", "kike", "kinbaku", "kinkster", "kinky", "kiunt", "knob", "knobbing", "knobead", "knobed", "knobend", "knobhead", "knobjocky", "knobjokey", "kock", "kondum", "kondums", "kooch", "kootch", "kum", "kumer", "kummer", "kumming", "kums", "kunilingus", "kunt", "kyke", "l3i+ch", "l3itch", "labia", "leather restraint", "leather straight jacket", "lemon party", "lesbo", "lezzie", "lmfao", "lolita", "lovemaking", "lust", "lusting", "m0f0", "m0fo", "m45terbate", "ma5terb8", "ma5terbate", "make me come", "male squirting", "masochist", "master-bate", "masterb8", "masterbat*", "masterbat3", "masterbate", "masterbation", "masterbations", "masturbate", "menage a trois", "milf", "minge", "missionary position", "mo-fo", "mof0", "mofo", "mothafuck", "mothafucka", "mothafuckas", "mothafuckaz", "mothafucked ", "mothafucker", "mothafuckers", "mothafuckin", "mothafucking ", "mothafuckings", "mothafucks", "mother fucker", "motherfuck", "motherfucked", "motherfucker", "motherfuckers", "motherfuckin", "motherfucking", "motherfuckings", "motherfuckka", "motherfucks", "mound of venus", "mr hands", "muff", "muff diver", "muffdiver", "muffdiving", "mutha", "muthafecker", "muthafuckker", "muther", "mutherfucker", "n1gga", "n1gger", "nambla", "nawashi", "nazi", "negro", "neonazi", "nig nog", "nigg3r", "nigg4h", "nigga", "niggah", "niggas", "niggaz", "nigger", "niggers ", "niglet", "nimphomania", "nipple", "nipples", "nob", "nob jokey", "nobhead", "nobjocky", "nobjokey", "nsfw images", "nude", "nudity", "numbnuts", "nutsack", "nympho", "nymphomania", "octopussy", "omorashi", "one cup two girls", "one guy one jar", "orgasim", "orgasim ", "orgasims ", "orgasm", "orgasms ", "orgy", "p0rn", "paedophile", "paki", "panooch", "panties", "panty", "pawn", "pecker", "peckerhead", "pedobear", "pedophile", "pegging", "penis", "penisfucker", "phone sex", "phonesex", "phuck", "phuk", "phuked", "phuking", "phukked", "phukking", "phuks", "phuq", "piece of shit", "pigfucker", "pimpis", "pis", "pises", "pisin", "pising", "pisof", "piss", "piss pig", "pissed", "pisser", "pissers", "pisses ", "pissflap", "pissflaps", "pissin", "pissin ", "pissing", "pissoff", "pissoff ", "pisspig", "playboy", "pleasure chest", "pole smoker", "polesmoker", "pollock", "ponyplay", "poo", "poof", "poon", "poonani", "poonany", "poontang", "poop", "poop chute", "poopchute", "porn", "porno", "pornography", "pornos", "prick", "pricks ", "prince albert piercing", "pron", "pthc", "pube", "pubes", "punanny", "punany", "punta", "pusies", "pusse", "pussi", "pussies", "pussy", "pussylicking", "pussys ", "pusy", "puto", "queaf", "queef", "queerbait", "queerhole", "quim", "raghead", "raging boner", "rape", "raping", "rapist", "rectum", "renob", "retard", "reverse cowgirl", "rimjaw", "rimjob", "rimming", "rosy palm", "rosy palm and her 5 sisters", "ruski", "rusty trombone", "s hit", "s&m", "s.o.b.", "s_h_i_t", "sadism", "sadist", "santorum", "scat", "schlong", "scissoring", "screwing", "scroat", "scrote", "scrotum", "semen", "sex", "sexo", "sexy", "sh!+", "sh!t", "sh1t", "shag", "shagger", "shaggin", "shagging", "shaved beaver", "shaved pussy", "shemale", "shi+", "shibari", "shit", "shit-ass", "shit-bag", "shit-bagger", "shit-brain", "shit-breath", "shit-cunt", "shit-dick", "shit-eating", "shit-face", "shit-faced", "shit-fit", "shit-head", "shit-heel", "shit-hole", "shit-house", "shit-load", "shit-pot", "shit-spitter", "shit-stain", "shitass", "shitbag", "shitbagger", "shitblimp", "shitbrain", "shitbreath", "shitcunt", "shitdick", "shite", "shiteating", "shited", "shitey", "shitface", "shitfaced", "shitfit", "shitfuck", "shitfull", "shithead", "shitheel", "shithole", "shithouse", "shiting", "shitings", "shitload", "shitpot", "shits", "shitspitter", "shitstain", "shitted", "shitter", "shitters ", "shittiest", "shitting", "shittings", "shitty", "shitty ", "shity", "shiz", "shiznit", "shota", "shrimping", "skank", "skeet", "slanteye", "slut", "slutbag", "sluts", "smeg", "smegma", "smut", "snatch", "snowballing", "sodomize", "sodomy", "son-of-a-bitch", "spac", "spic", "spick", "splooge", "splooge moose", "spooge", "spread legs", "spunk", "strap on", "strapon", "strappado", "strip club", "style doggy", "suck", "sucks", "suicide girls", "sultry women", "swastika", "swinger", "t1tt1e5", "t1tties", "tainted love", "tard", "taste my", "tea bagging", "teets", "teez", "testical", "testicle", "threesome", "throating", "thundercunt", "tied up", "tight white", "tit", "titfuck", "tits", "titt", "tittie5", "tittiefucker", "titties", "titty", "tittyfuck", "tittywank", "titwank", "tongue in a", "topless", "tosser", "towelhead", "tranny", "tribadism", "tub girl", "tubgirl", "turd", "tushy", "tw4t", "twat", "twathead", "twatlips", "twatty", "twink", "twinkie", "two girls one cup", "twunt", "twunter", "undressing", "upskirt", "urethra play", "urophilia", "v14gra", "v1gra", "va-j-j", "vag", "vagina", "venus mound", "viagra", "vibrator", "violet wand", "vjayjay", "vorarephilia", "voyeur", "vulva", "w00se", "wang", "wank", "wanker", "wanky", "wet dream", "wetback", "white power", "whoar", "whore", "willies", "willy", "wrapping men", "wrinkled starfish", "xrated", "xx", "xxx", "yaoi", "yellow showers", "yiffy", "zoophilia" };

        // Create a regular expression pattern to match whole words
        string pattern = @"\b(" + string.Join("|", badWords) + @")\b";

        // Use Regex.IsMatch to check for matches
        return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
    }
    IEnumerator tutorialRing()
    {
        commandField.interactable = false;

        while (!TerminalPanel.activeInHierarchy)
        {
            vfxsource.PlayOneShot(notificationClip);
            terminalButtonImage.color = Color.magenta;
            yield return new WaitForSeconds(0.5f);
            terminalButtonImage.color = Color.white;
            yield return new WaitForSeconds(1f);
        }

        yield return StartCoroutine(tutorialSay("hello detective"));
        yield return StartCoroutine(tutorialSay("long time no see"));
        yield return StartCoroutine(tutorialSay("don't worry, it's alex"));
        yield return StartCoroutine(tutorialSay("i just did the updates to the system you requested and thought i might guide you through them"));
        yield return StartCoroutine(tutorialSay("alright, I will enable a simple command: say. try using it"));
        yield return new WaitForSeconds(0.5f);
        terminal.unlockCommand(3, true);
        yield return StartCoroutine(tutorialSay("just type 'say hello' or something and press the enter key"));
        yield return StartCoroutine(tutorialSay("also type 'say skip' if you don't need my help"));

        commandField.interactable = true; commandField.Select();
        while (!terminal.statement.ToLower().Contains("say "))
        {
            yield return new WaitForSeconds(0.5f);
        }
        commandField.interactable = false;

        if (ContainsBadWord(terminal.statement.ToLower()))
        {
            yield return StartCoroutine(tutorialSay("very funny -_-"));
        }

        if (!terminal.statement.ToLower().Contains("skip"))
        {


            yield return StartCoroutine(tutorialSay("alright let's do some file navigation"));
            yield return StartCoroutine(tutorialSay("try the list command, just type in 'list'"));
            yield return new WaitForSeconds(0.5f);
            terminal.unlockCommand(4, true);
            commandField.interactable = true; commandField.Select();
            while (!terminal.statement.ToLower().Equals("list"))
            {
                yield return new WaitForSeconds(0.5f);
            }
            commandField.interactable = false;
            yield return StartCoroutine(tutorialSay("'list' allows you to view files and directories"));
            yield return StartCoroutine(tutorialSay("directory names will be colored in cyan while files will be colored white"));
            yield return StartCoroutine(tutorialSay("you can also list other directories without going there by typing the path to the directory"));
            yield return StartCoroutine(tutorialSay("try 'list pictures'"));
            commandField.interactable = true; commandField.Select();
            
            while (!terminal.statement.ToLower().Equals("list pictures"))
            {
                yield return new WaitForSeconds(0.5f);
            }
            commandField.interactable = false;
            yield return StartCoroutine(tutorialSay("great, you're getting the hang of this"));
            yield return StartCoroutine(tutorialSay("let's try something else"));
            yield return StartCoroutine(tutorialSay("type 'goto documents'"));
            terminal.unlockCommand(5, true);
            bool checker = false;
            commandField.interactable = true; commandField.Select();
            while (!terminal.statement.ToLower().Equals("goto documents"))
            {
                if (terminal.statement.ToLower().Contains("goto") && !checker)
                {
                    checker = true;
                    yield return StartCoroutine(tutorialSay("it seems you went to the wrong directory, use goto / to go back to the root directory"));
                }
                yield return new WaitForSeconds(0.5f);
            }
            commandField.interactable = false;
            yield return StartCoroutine(tutorialSay("some files and directories will be protected by a password"));
            yield return StartCoroutine(tutorialSay("this document requested a password"));
            yield return StartCoroutine(tutorialSay("enter the password 'iluvalex'"));
            commandField.interactable = true; commandField.Select();
            while (!terminal.fileSystem.currentDirectory.filename.Equals("documents"))
            {
                yield return new WaitForSeconds(0.5f);
            }
            commandField.interactable = false;
            yield return StartCoroutine(tutorialSay("aww thank you ;p"));
            yield return StartCoroutine(tutorialSay("anyway there's a text file in the documents folder"));
            yield return StartCoroutine(tutorialSay("use the read command to read it"));
            yield return StartCoroutine(tutorialSay("it will open the file in a text viewer"));
            yield return StartCoroutine(tutorialSay("don't forget to use the command 'list' first to check the file name"));
            yield return StartCoroutine(tutorialSay("then type 'read <file_name>' to open the file"));
            yield return StartCoroutine(tutorialSay("open the terminal again when you're done"));
            terminal.unlockCommand(6, true);
            commandField.interactable = true; commandField.Select();
            while (!docviewer.text.Contains("pina colada"))
            {
                yield return new WaitForSeconds(0.5f);
            }
            commandField.interactable = false;
            while (!TerminalPanel.activeInHierarchy)
            {
                vfxsource.PlayOneShot(notificationClip);
                terminalButtonImage.color = Color.magenta;
                yield return new WaitForSeconds(0.5f);
                terminalButtonImage.color = Color.white;
                yield return new WaitForSeconds(1f);
            }
            yield return StartCoroutine(tutorialSay("you're getting the hang of this"));
            yield return StartCoroutine(tutorialSay("let's try the search command"));
            yield return StartCoroutine(tutorialSay("type 'search \"coffee\"' and make sure to include the quotation marks around \"coffee\""));
            terminal.unlockCommand(10, true);
            commandField.interactable = true; commandField.Select();
            while (!terminal.statement.ToLower().Equals("search \"coffee\""))
            {
                yield return new WaitForSeconds(0.5f);
            }
            commandField.interactable = false;
            yield return StartCoroutine(tutorialSay("let's try to open that coffee file"));
            yield return StartCoroutine(tutorialSay("you can open it directly by useing 'read /pictures/coffee'"));
            yield return StartCoroutine(tutorialSay("starting a path with / means you're entering the full path from the root directory"));
            commandField.interactable = true; commandField.Select();
            while (!imageviewer.sprite.name.Equals("detective"))
            {
                yield return new WaitForSeconds(0.5f);
            }
            commandField.interactable = false;
            while (!TerminalPanel.activeInHierarchy)
            {
                vfxsource.PlayOneShot(notificationClip);
                terminalButtonImage.color = Color.magenta;
                yield return new WaitForSeconds(0.5f);
                terminalButtonImage.color = Color.white;
                yield return new WaitForSeconds(1f);
            }
            yield return StartCoroutine(tutorialSay("don't we look super cute in that picture X_X"));
            yield return StartCoroutine(tutorialSay("alright, let's check your email"));
            yield return StartCoroutine(tutorialSay("just type in the command word 'email'"));
            commandField.interactable = true; commandField.Select();
            terminal.unlockCommand(9, true);
            while (!terminal.statement.ToLower().Equals("email") && !terminal.statement.ToLower().Contains("email "))
            {
                yield return new WaitForSeconds(0.5f);
            }
            commandField.interactable = false;

            while (!TerminalPanel.activeInHierarchy)
            {
                vfxsource.PlayOneShot(notificationClip);
                terminalButtonImage.color = Color.magenta;
                yield return new WaitForSeconds(0.5f);
                terminalButtonImage.color = Color.white;
                yield return new WaitForSeconds(1f);
            }
            yield return StartCoroutine(tutorialSay("looks like you've got mail"));
            yield return StartCoroutine(tutorialSay("you can use the same command 'email' with the number or the full title of the email to open it in doc viewer"));
            yield return StartCoroutine(tutorialSay("i'd go for number personally XD"));
            if (!terminal.statement.ToLower().Equals("email 1"))
            {
                yield return StartCoroutine(tutorialSay("try 'email 1' to open the first email in the list"));
                commandField.interactable = true; commandField.Select();
                while (!docviewer.text.Contains("Sarah Johnson"))
                {
                    yield return new WaitForSeconds(0.5f);
                }
                commandField.interactable = false;
                while (!TerminalPanel.activeInHierarchy)
                {
                    vfxsource.PlayOneShot(notificationClip);
                    terminalButtonImage.color = Color.magenta;
                    yield return new WaitForSeconds(0.5f);
                    terminalButtonImage.color = Color.white;
                    yield return new WaitForSeconds(1f);
                }
            }
            yield return StartCoroutine(tutorialSay("looks like that woman you told me about"));
            yield return StartCoroutine(tutorialSay("she wants you to access her fiance's computer"));
            terminal.unlockCommand(1, true);
            yield return StartCoroutine(tutorialSay("now you can use the connect command to unlock it"));
            yield return StartCoroutine(tutorialSay("just type 'connect' and the PAP address she sent"));
            yield return StartCoroutine(tutorialSay("then it will ask you for the password"));
            yield return StartCoroutine(tutorialSay("if you did not copy it the email should still be open on the doc viewer on the right"));
            yield return StartCoroutine(tutorialSay("you can use ctrl + c to copy the text"));
            DocViewer.interactable = true;
            yield return StartCoroutine(tutorialSay("try it"));
            commandField.interactable = true; commandField.Select();
            while (!terminal.fileSystem.personName.Equals("Blackwood - Home"))
            {
                yield return new WaitForSeconds(0.5f);
            }
            yield return StartCoroutine(tutorialSay("seems you managed to connect"));
            yield return StartCoroutine(tutorialSay("alright I gotta leave now"));
            yield return StartCoroutine(tutorialSay("you can navigate his computer the same way"));
            yield return StartCoroutine(tutorialSay("i'll unlock the rest of the commands for you"));
            terminal.unlockCommand(0, true);
            terminal.unlockCommand(2, true);
            terminal.unlockCommand(8, true);
            terminal.unlockCommand(11, true);
            yield return StartCoroutine(tutorialSay("make sure to use the 'help' command to read about them"));
            yield return StartCoroutine(tutorialSay("also there is a program called ProFile on the right"));
            int counter = 0;
            while (counter <= 3)
            {
                counter++;
                vfxsource.PlayOneShot(notificationClip);
                profileimage.color = Color.magenta;
                yield return new WaitForSeconds(0.5f);
                profileimage.color = Color.white;
                yield return new WaitForSeconds(1f);
            }
            yield return StartCoroutine(tutorialSay("use it to switch to computers you have already unlocked"));
            yield return StartCoroutine(tutorialSay("one last thing, you can take notes on each computer using the ProFile app"));
            yield return StartCoroutine(tutorialSay("there is also a note pad on the top left"));
            yield return StartCoroutine(tutorialSay("good luck with your case ;)"));
            Profile.interactable = true;
            ImageViewer.interactable = true;
            DocViewer.interactable = true;
            AudioPlayer.interactable = true;
            Settings.interactable = true;
        }
        else
        {
            commandField.interactable = true; commandField.Select();

            yield return StartCoroutine(tutorialSay("alright, no problem"));
            yield return StartCoroutine(tutorialSay("i've unlocked all the commands"));
            yield return StartCoroutine(tutorialSay("good luck with your case ;)"));
            terminal.EnterResponse("user alex has logged off");
            Profile.interactable = true;
            ImageViewer.interactable = true;
            DocViewer.interactable = true;
            AudioPlayer.interactable = true;
            Settings.interactable = true;
            Decrypt.gameObject.SetActive(true);
            Decrypt.interactable = true;
            terminal.unlockAllCommands();


        }
    }

public IEnumerator tutorialSay(string say)
    {
        terminal.EnterResponse("alex is typing...");
        float time = ((float)say.Length) / 10f;
        yield return new WaitForSeconds(time);
        terminal.removeText("\nalex is typing...");
        tutorialCharacter(say);
    }
    public void tutorialCharacter(string say)
    {
        vfxsource.PlayOneShot(successClip);
        terminal.EnterResponse("<b><color=#e8d500>alex:  </color></b>" + say);
    }

    public IEnumerator victorSay(string say)
    {
        terminal.EnterResponse("victor is typing...");
        float time = ((float)say.Length) / 10f;
        yield return new WaitForSeconds(time);
        terminal.removeText("\nvictor is typing...");
        victorCharacter(say);
    }
    public void victorCharacter(string say)
    {
        vfxsource.PlayOneShot(successClip);
        terminal.EnterResponse("<b><color=#bf0202>victor:  </color></b>" + say);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator dekryptTutorial()
    {
        
            commandField.interactable = false;
            Profile.interactable = false;
            ImageViewer.interactable = false;
            DocViewer.interactable = false;
            AudioPlayer.interactable = false;
            Settings.interactable = false;
            Decrypt.interactable = false;

            while (!TerminalPanel.activeInHierarchy)
            {
                vfxsource.PlayOneShot(notificationClip);
                terminalButtonImage.color = Color.magenta;
                yield return new WaitForSeconds(0.5f);
                terminalButtonImage.color = Color.white;
                yield return new WaitForSeconds(1f);
            }
            terminal.EnterResponse("user alex has logged on");
            yield return StartCoroutine(tutorialSay("hello detective"));
            yield return StartCoroutine(tutorialSay("it seems you have run into an encrypted file"));
            yield return StartCoroutine(tutorialSay("luckily I have just the program for that"));
            yield return StartCoroutine(tutorialSay("let me enable it"));
            Decrypt.gameObject.SetActive(true);
            terminal.unlockCommand(7, true);
            yield return StartCoroutine(tutorialSay("this is the dekrypt command"));
            yield return StartCoroutine(tutorialSay("you can use it to decrypt files"));
            yield return StartCoroutine(tutorialSay("just type 'dekrypt <file path>' in the terminal, the same way you use 'read'"));
            yield return StartCoroutine(tutorialSay("then you'll get the encrypted file in the program"));
            yield return StartCoroutine(tutorialSay("use the mouse wheel to cycle through the letter"));
            yield return StartCoroutine(tutorialSay("small hint: start with words that you already know"));
            yield return StartCoroutine(tutorialSay("I think the second word in that file is password ;)"));
            yield return StartCoroutine(tutorialSay("also try to find common words like 'the' or 'is'"));
            yield return StartCoroutine(tutorialSay("once you change a letter all the corresponding letters will change as well"));
            yield return StartCoroutine(tutorialSay("I have also added a detailed explanation of how it works on your computer"));
            yield return StartCoroutine(tutorialSay("I'll open it right now, it's also saved on your system"));
            yield return StartCoroutine(tutorialSay("good luck :)"));
            terminal.EnterResponse("user alex has logged off");
            yield return new WaitForSeconds(1f);
            terminal.OpenImageDoc(new ImageDocument("dekrypt_tutorial", "", "Sprites/dekrypt_tutorial", ""));
        


    }
    IEnumerator finalRing()
    {
        MainCamera.GetComponent<CameraFilterPack_FX_Glitch3>().enabled = true;
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(Random.Range(0f,1f));
            vfxsource.PlayOneShot(failClip);
        }
        
        MainCamera.GetComponent<CameraFilterPack_FX_Glitch3>().enabled = false;


        terminal.lockAllCommands();
        commandField.interactable = false;
        Profile.interactable = false;
        ImageViewer.interactable = false;
        DocViewer.interactable = false;
        AudioPlayer.interactable = false;
        Settings.interactable = false;
        Decrypt.interactable = false;

        while (!TerminalPanel.activeInHierarchy)
        {
            vfxsource.PlayOneShot(notificationClip);
            terminalButtonImage.color = Color.magenta;
            yield return new WaitForSeconds(0.5f);
            terminalButtonImage.color = Color.white;
            yield return new WaitForSeconds(1f);
        }

        yield return StartCoroutine(victorSay("hello detective"));
        yield return StartCoroutine(victorSay("it seems there is no escaping you"));
        yield return StartCoroutine(victorSay("i assume you understand why this had to be done"));
        yield return StartCoroutine(victorSay("or why i think that"));
        yield return StartCoroutine(victorSay("maybe you disagree"));
        yield return StartCoroutine(victorSay("i've been watching your progress, i know you have all the facts"));
        yield return StartCoroutine(victorSay("adrian's research had to be stopped"));
        yield return StartCoroutine(victorSay("Viridian, A-Corp had to be stopped"));
        yield return StartCoroutine(victorSay("do you understand the power one company could have with a weapon based on Viridian?"));
        yield return StartCoroutine(victorSay("have you seen the documents that show they were testing it on humans?"));
        yield return StartCoroutine(victorSay("do you understand how one genocidal monster can use it to wipe a whole culture off the face of the earth?"));
        yield return StartCoroutine(victorSay("so I'll admit it"));
        yield return StartCoroutine(victorSay("it was me, I did it"));
        yield return StartCoroutine(victorSay("sinclair wasn't able to so I went there and killed adrian myself"));
        yield return StartCoroutine(victorSay("with his own research nonetheless"));
        yield return StartCoroutine(victorSay("knowing full well that the ScienceWorks and A-Corp would not investigate"));
        yield return StartCoroutine(victorSay("launching an investigation with a \"lab accident\" like this would mean their end"));
        yield return StartCoroutine(victorSay("so they covered it up"));
        yield return StartCoroutine(victorSay("but you uncovered it"));
        yield return StartCoroutine(victorSay("and now it's all in your hand"));
        yield return StartCoroutine(victorSay("to be honest, I don't see myself in a position to determine the future of this research"));
        yield return StartCoroutine(victorSay("and I understand that Blackwood's loved ones deserve to know"));
        yield return StartCoroutine(victorSay("so I will leave it to you"));
        yield return StartCoroutine(victorSay("the way I see it you have four options"));
        yield return StartCoroutine(victorSay("the first option is to send your findings to the police"));
        yield return StartCoroutine(victorSay("I will be arrested, sinclair too, Blackwood's fiance will know the whole truth"));
        yield return StartCoroutine(victorSay("but I am certain all the data about Viridian and A-Corp will be covered up"));
        yield return StartCoroutine(victorSay("option 2 will be to send your finding to a journalist"));
        yield return StartCoroutine(victorSay("I know one mr. frank cunningham who works at the national"));
        yield return StartCoroutine(victorSay("he will gladly take your story and expose A-Corp"));
        yield return StartCoroutine(victorSay("the third option is contacting A-Corp directly"));
        yield return StartCoroutine(victorSay("all of this will be covered up, and nothing will come of it"));
        yield return StartCoroutine(victorSay("except my untimely death, probably"));
        yield return StartCoroutine(victorSay("finally, you can leave it all be"));
        yield return StartCoroutine(victorSay("nothing will change, no one will know the truth"));
        yield return StartCoroutine(victorSay("and Viridian research will continue"));
        yield return StartCoroutine(victorSay("it's your choice, detective"));
        yield return StartCoroutine(victorSay("i'll set up the commands to send the data"));
        terminal.endingCommands();
        commandField.interactable = true;

        while (!terminal.finalWait)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return StartCoroutine(victorSay("i guess you made your choice"));
        yield return StartCoroutine(victorSay("can you live with it?"));
        yield return StartCoroutine(victorSay("i won't do anything to stop whatever comes"));
        yield return StartCoroutine(victorSay("i just hope it was the right choice"));
        yield return StartCoroutine(victorSay("goodbye, detective"));
        terminal.EnterResponse("user victor has logged off");


    }

}
