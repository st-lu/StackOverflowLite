import pandas as pd
import random

slurs_dict = {'fuck': ['f***', 'f***ing', 'f**k', 'f*ck', 'fuk'],
 'dick': ['d!ck', 'd!k', 'd***', 'd*ck', 'd1ck'],
 'cunt': ['c***', 'c**t', 'c*nt', 'c*ntface', 'c*nts'],
 'nigga': ['n****', 'n****r', 'n***a'],
 'faggot': ['f***ot', 'f*ggot', 'f4ggot', 'f@ggot', 'fa***t'],
 'whore': ['w#ore', 'w*ore', 'wh***', 'wh0re', 'wh@re'],
 'bastard': ['b***ard', 'b*stard', 'b4stard', 'b@stard', 'ba$tard'],
 'pussy': ['p!ssy', 'p***y', 'p*ssy', 'pu$$i', 'pu$$y'],
 'dumbass': ['dumass', 'dumb***', 'dumb@ss', 'dumba$$', 'dumba**'],
 'shit': ['s**t', 'sh!t', 'sh***', 'sh*t', 'sh1t'],
 'asshole': ['a$$hole', 'a**hole', 'a-hole', 'a55hole', 'arsehole'],
 'motherfucker': ['m****f****r','m***rf**ker','m*therf*cker','mofo','mutha****a'],
 'cocksucker': ['c***sucker','c**ksucker','c*cksucker','c0ck$ucker','c0cksucker'],
 'bitch': ['b!tc#', 'b!tch', 'b***h', 'b*tch', 'b1tch'],
 'dickhead': ['d!ckhead', 'd!ckhed', 'd***head', 'd*ckhead', 'd1ckhead'],
 'prick': ['pr!ck', 'pr!k', 'pr***', 'pr*ck', 'pr1ck'],
 'retard': ['r***rd', 'r*tard', 'r3t@rd', 'r3tard', 're***d'],
 'slut': ['s!ut', 'sl***', 'sl*t', 'sl4t', 'sl@t'],
 'bastards': ['b***ards', 'b*stards', 'b4stards', 'b@stards', 'ba$tards'],
 'shithead': ['sh!the@d', 'sh!thead', 'sh***head', 'sh*thead', 'sh1thead'],
 'wanker': ['w***er', 'w*anker', 'w4nker', 'w4nkr', 'w@nker'],
 'fuckface': ['f***f@ce', 'f***face', 'f*ckface', 'fuc*face', 'fuc4face'],
 'twat': ['tw***', 'tw*t', 'tw4t', 'tw@t', 'tw@tt'],
 'bitchass': ['b!tchass', 'b***hass', 'b*tchass', 'b1tchass', 'bitch@$$'],
 'ass': ['4ss', '@ss', 'a$$', 'a**', 'a55'],
 'asswipe': ['4sswipe', '@sswipe', 'a$$wipe', 'a**wipe', 'a55wipe'],
 'fucked': ['f***d', 'f***ed', 'f**ked', 'f*cked', 'fuked'],
 'fuckhead': ['f***head', 'f**khead', 'f*ckhead', 'fukhead', 'fukhed'],
 'nigger': ['n***er', 'n**ger', 'n*gga', 'n1gga', 'n1gger'],
 'piss': ['p!ss', 'p***', 'p*ss', 'p1ss', 'pi$$'],
 'pissed': ['p!ssed', 'p***ed', 'p*ssed', 'p1ssed', 'pi$$ed'],
 'shitface': ['sh!tface', 'sh***face', 'sh*tface', 'sh*thed', 'sh1tface'],
 'douche': ['d***bag', 'd***he', 'd*uche', 'd0uche', 'douchebag'],
 'jackass': ['j***ass', 'j*ckass', 'j4ckass', 'j@ckass', 'jack@$$'],
 'ballsack': ['b***sack', 'b*llsack', 'b4llsack', 'b@ll$ack', 'b@llsack'],
 'scumbag': ['sc***bag', 'sc*mbag', 'scu***', 'scum***', 'scumb@g'],
 'fucktard': ['f***t@rd', 'f***tard', 'f**ktard', 'f*cktard', 'fuktard'],
 'shitbag': ['sh!tbag', 'sh***bag', 'sh*tbag', 'sh1tb@g', 'sh1tbag'],
 'assclown': ['4ssclown', 'a$$clown', 'a**clown', 'a55clown', 'asscl*wn']

        }



def augment_text_with_all_slurs(text):
    words = text.split()
    augmented_texts = [text]

    for i, word in enumerate(words):
        lower_word = word.lower()
        if lower_word in slurs_dict:
            new_texts = []
            for replacement in slurs_dict[lower_word]:
                for augmented_text in augmented_texts:
                    new_words = augmented_text.split()
                    new_words[i] = replacement.capitalize() if word[0].isupper() else replacement
                    new_texts.append(" ".join(new_words))
            augmented_texts = new_texts

    return augmented_texts

csv_file = "dataset_hatespeech.csv"
output_file = "dataset_hatespeech.csv"
dataset = pd.read_csv(csv_file)

augmented_rows = []

for _, row in dataset.iterrows():
    augmented_texts = augment_text_with_all_slurs(row['tweet'])
    for augmented_text in augmented_texts:
        augmented_rows.append({"class": row['class'], "tweet": augmented_text})

augmented_dataset = pd.DataFrame(augmented_rows)

augmented_dataset.to_csv(output_file, index=False)

