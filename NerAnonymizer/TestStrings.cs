namespace NerAnonymizer;

public static class TestStrings
{

    public const string NewsString = """
    Turkin parlamentti hyväksyi Ruotsin Nato-jäsenyyden

    Turkin parlamentti on pitkän keskustelun jälkeen hyväksynyt Ruotsin Nato-hakemuksen.

    Äänestysluvut olivat 287-55 jäsenyyden puolesta.

    Ratifiointi menee vielä presidentti Recep Tayyip Erdoganin hyväksyttäväksi.

    Turkin annettua hyväksyntänsä Unkari on ainoa maa, joka ei vielä ole hyväksynyt Ruotsin jäsenyyttä.
    """;

    public const string ShortTestString = """
Asiassa oli ratkaistavana kysymys siitä, oliko Puolustusvoimien aluetoimisto voinut hylätä sukupuoltaan miehestä naiseksi korjanneen valittajan hakemuksen hänen asevelvollisuutensa lakkauttamisesta ja reservistä poistamisesta. Hallinto-oikeus kumosi valituksenalaisen päätöksen lainvastaisena ja palautti asian aluetoimiston uudelleen käsiteltäväksi.
""";

    public const string LongTestString = """
Asiassa oli ratkaistavana kysymys siitä, oliko Puolustusvoimien aluetoimisto voinut hylätä sukupuoltaan miehestä naiseksi korjanneen valittajan hakemuksen hänen asevelvollisuutensa lakkauttamisesta ja reservistä poistamisesta. Hallinto-oikeus kumosi valituksenalaisen päätöksen lainvastaisena ja palautti asian aluetoimiston uudelleen käsiteltäväksi.

Asiassa saadusta selvityksestä kävi ilmi, että valittaja oli ennen sukupuolen korjaustaan suorittanut asevelvollisuuslain (452/1950) 1 §:n 1 momentin mukaisen varusmiespalveluksen juridisesti miehenä 1990-luvulla, jonka jälkeen hän oli siirtynyt asevelvollisuuslain mukaisesti reserviin. Valittaja oli reserviin siirtymisen jälkeen korjannut sukupuolensa, jolloin hänestä oli tullut sukupuoli-identiteettinsä mukaisesti myös juridisesti nainen. Valittaja oli vuonna 2022 hakenut aluetoimistolta henkilökohtaisen asevelvollisuutensa lakkauttamista ja poistamistaan reservistä, koska hän oli halunnut tulla kohdelluksi juridisen sukupuolensa mukaisesti. Aluetoimisto oli perustanut hakemuksen hylkäävän päätöksensä maavoimien esikunnan antaman normikokoelman määräykseen ja pääesikunnan oikeudellisen osaston kannanottoon, jonka mukaan koska valittaja oli jo kokonaisuudessaan suorittanut varusmiespalveluksen, ei lainsäädäntö mahdollistanut asevelvollisuuden lakkauttamista ja reservistä poistamista. Asiassa oli hallinto-oikeuden ratkaisun enemmistön mukaan kysymys hallinto-oikeudessa ennen kaikkea siitä, voitiinko aluetoimiston tekemän päätöksen, jolla se oli hylännyt valittajan vaatimuksen hänen henkilökohtaisen asevelvollisuutensa lakkaamisesta ja reservistä poistamisesta, katsoa olevan tasa-arvolain ja transseksuaalin sukupuolen vahvistamisesta annetun lain (563/2002) vastainen siten, että valituksenalainen päätös tosiasiallisesti johti sukupuoli-identiteettiin perustuvaan syrjintään.

Hallinto-oikeus katsoi, että valittajan asevelvollisuus oli perustunut siihen, että ollessaan juridisesti mies, hän oli asevelvollisuuslain nojalla tullut asevelvolliseksi, suorittanut varusmiespalveluksen ja siirtynyt reserviin. Naisten vapaaehtoisesta asepalveluksesta annetussa laissa säädettiin naisten mahdollisuudesta hakea varusmiespalvelusta vastaavaan vapaaehtoiseen asepalvelukseen ja siitä, kuinka naisten vapaaehtoisen asepalveluksen suorittanut nainen rinnastettiin asevelvollisuuslain mukaiseen asevelvolliseen. Voimassa olevien säännösten mukaisesti henkilö tuli asevelvolliseksi joko asevelvollisuuslain 2 §:n 2 momentin mukaisesti ollessaan miespuolinen Suomen kansalainen, tai naisten vapaaehtoisesta asepalveluksesta annetun lain 6 §:n 3 momentin perusteella suorittaessaan tarkemmin laissa säädetyllä tavalla naisten vapaaehtoisen asepalveluksen. Naisten vapaaehtoisesta asepalveluksesta annetussa laissa ei ollut säännöksiä sukupuolensa korjanneiden asemasta. Hallinto-oikeus totesi, ettei valittaja ollut suorittanut tai edes hakenut naisten vapaaehtoisesta asepalveluksesta annetun lain mukaista asepalvelusta, eikä valittajan tilanteessa voitu siten soveltaa naisten vapaaehtoisesta asepalveluksesta annettuja säännöksiä. Koska laissa ei ollut toisin säädetty, valittajaa ei voitu täten rinnastaa naiseen, joka oli suorittanut naisten vapaaehtoisen asepalveluksen ja tullut siten naisten vapaaehtoisesta asepalveluksesta annetussa laissa säädetyllä tavalla asevelvolliseksi.

Hallinto-oikeus totesi, että valituksenalaisen päätöksen tekohetkellä voimassa olleen transseksuaalin sukupuolen vahvistamisesta annetun lain (563/2002) 5 §:n mukaan vahvistettua sukupuolta oli pidettävä henkilön sukupuolena sovellettaessa muuta lainsäädäntöä, jollei toisin säädetä. Lain tavoitteena oli ollut, että henkilö, jonka sukupuoli oli vahvistettu, tulee kohdelluksi uuden sukupuolensa mukaisella tavalla sovellettaessa muuta lainsäädäntöä. Lain esitöissä ei ollut tarkemmin arvioitu säännöksen vaikutuksia asevelvollisuuslain soveltamiseen. Sen sijaan sukupuolen vahvistamisesta annetun lain (295/2023) esitöissä oli nimenomaisesti painotettu sitä, että uuden lain mukaan vahvistettua sukupuolta tuli soveltaa myös asevelvollisuuslakia sovellettaessa ja lain esitöissä oli muun muassa arvioitu lain mahdollisia vaikutuksia asevelvollisuuteen. Transseksuaalin sukupuolen vahvistamisesta annettu laki oli ollut oikeusvaikutustensa osalta sisällöltään ja tavoitteiltaan samanlainen kuin uusi sukupuolen vahvistamisesta annettu laki, eikä transseksuaalin sukupuolen vahvistamisesta annetussa laissa ollut poikkeuksia asevelvollisuuden suhteen. Myöskään asevelvollisuuslaissa ei ollut poikkeavia säännöksiä siitä, kuinka asevelvollisuuslakia ja siten asevelvollisuuslakiin perustuvaa asevelvollisuutta tuli soveltaa sukupuolensa korjanneisiin henkilöihin. Sekä transseksuaalin sukupuolen vahvistamisesta annetun lain että tasa-arvolain säännökset toteuttivat osaltaan perustuslain 6 §:n 2 momentissa turvattua oikeutta tulla kohdelluksi yhdenvertaisesti. Ottaen huomioon perustuslain ja tasa-arvolain säännökset ja koska asevelvollisuuslaissa ei ollut nimenomaisesti toisin säädetty, hallinto-oikeus katsoi, että myös valituksenalaisen päätöksen tekohetkellä voimassa olleen transseksuaalin sukupuolen vahvistamisesta annetun lain 5 §:n perusteella valittajaa oli tullut kohdella päätöstä tehtäessä hänen juridisen sukupuolensa mukaisesti naisena asevelvollisuuslakia sovellettaessa.

Maavoimien esikunnan 8.6.2022 antaman määräyksen 11.1.3 kohdan mukaan, jos henkilön sukupuolen vaihdos oli vahvistettu varusmiespalveluksen jälkeen, hänen reserviläisstatuksensa jatkui kuten varusmiespalveluksen suorittaneilla naisillakin. Valituksenlainen päätös oli perustunut kyseiseen maavoimien esikunnan antamaan määräykseen, jossa varusmiespalveluksen jo suorittaneet, myöhemmin sukupuolensa miehestä naiseksi korjanneet henkilöt, oli rinnastettu vapaaehtoisen varusmiespalveluksen suorittaneisiin ja siten asevelvollisiksi tulleisiin ja reserviin siirrettyihin naisiin. Valittajan tilanteessa ei ollut voitu kuitenkaan soveltaa naisten vapaaehtoisesta asepalveluksesta annettua lakia, koska valittaja ei ollut käynyt kyseisen lain mukaista asepalvelusta, vaan valittaja oli tullut asevelvolliseksi suoraan asevelvollisuuslain nojalla ollessaan juridisesti mies. Sekä valittajan varusmiespalveluksen suorittamisen aikaan voimassa olleen asevelvollisuuslain, että nykyisen voimassa olevan asevelvollisuuslain mukaan varusmiespalveluksen suorittanut asevelvollinen siirrettiin reserviin. Asiassa oli siten selvää, että valittaja oli kuulunut varusmiespalveluksen suorittamisen jälkeen reserviin. Asevelvollisuuslain 2 §:n 2 momentin mukaisesti varusmiespalvelus oli yksi osa asevelvollisuuden suorittamista, ja valittaja oli suorittanut miehille tarkoitetun asevelvollisuuslain mukaisen varusmiespalveluksen yhtenä osana miehille kuuluvaa asevelvollisuutta. Asevelvollisuuden ja reserviläisaseman oli katsottava siten olevan kytköksissä toisiinsa. Koska valittajan ei lähtökohtaisesti voitu katsoa olevan sukupuolensa perusteella enää asevelvollisuuslain mukaisesti asevelvollinen, ei hänen siten myöskään tässä tilanteessa voitu katsoa lähtökohtaisesti kuuluvan varusmiespalveluksen suorittaneiden asevelvollisten tapaan reserviin. Hallinto-oikeuden mukaan muunlainen tulkinta johtaisi tosiasiassa transseksuaalin sukupuolen vahvistamisesta annetun lain ja tasa-arvolain vastaiseen tilanteeseen, jossa valittaja ei tulisi kohdelluksi sukupuoli-identiteettinsä mukaisesti lain edellyttämällä tavalla. Hallinto-oikeus katsoi edellä mainitut perustelut huomioiden, että maavoimien esikunnan 8.6.2022 antaman määräyksen 11.1.3 kohta oli siten ristiriidassa lain kanssa siltä osin, kuin se rinnasti valittajan naiseen, joka oli suorittanut naisten vapaaehtoisen asepalveluksen. Kun otettiin huomioon perustuslain säännökset ja edellä mainitut laintasoiset säännökset, ei maavoimien esikunnan antamalle lakia alemman asteiselle määräykselle voitu antaa asiaa ratkaistaessa merkitystä. Valittajan hakemusta ei siten voitu hylätä tältä osin lain kanssa ristiriitaisen hallinnollisen määräyksen perusteella.

Hallinto-oikeus katsoi lisäksi, ettei asevelvollisuuslaissa ollut säännöksiä siitä, kuinka asevelvollisuuslakia tuli soveltaa tilanteessa, jossa henkilö on korjannut sukupuoltaan. Asevelvollisuuslaissa ei ollut myöskään säännöksiä menettelystä jo kertaalleen lain perusteella syntyneen asevelvollisuuden lakkauttamiseksi kokonaan tai reservistä poistamiseksi kyseessä olevassa tilanteessa. Yllä mainittu huomioiden asiassa oli kuitenkin pidettävä selvänä, että valittajaa oli päätöstä tehdessä tullut kohdella hänen sukupuoli-identiteettinsä ja juridisen sukupuolensa mukaisesti naisena. Häntä ei voitu pitää siten asevelvollisuuslain mukaisesti asevelvollisena, eikä valittajaan ollut voitu päätöstä tehtäessä soveltaa asevelvollisuuslain mukaisia asevelvollista koskevia säännöksiä. Perustuslain 21 § turvaa jokaiselle oikeuden saada asiansa käsitellyksi asianmukaisesti toimivaltaisessa tuomioistuimessa tai muussa viranomaisessa. Hallinto-oikeus katsoi, ettei menettelyllisten säännösten puute voinut olla tässä tilanteessa päätöksen perusteena laissa turvattujen oikeuksien toteutumiselle.

Perustuslain 6 §:n 2 momentin mukaan ketään ei saa ilman hyväksyttävää perustetta asettaa eri asemaan sukupuolensa perusteella. Tasa-arvolain 7 §:n 2 momentin 3 kohdan mukaan välittömällä sukupuoleen perustuvalla syrjinnällä tarkoitetaan tässä laissa eri asemaan asettamista sukupuoli-identiteetin tai sukupuolen ilmaisun perusteella. Transseksuaalin sukupuolen vahvistamisesta annetun lain 5 §:n mukaan lain mukaisesti vahvistettua sukupuolta oli tullut pitää henkilön sukupuolena sovellettaessa muuta lainsäädäntöä, jollei toisin ole säädetty. Asevelvollisuuslaissa ei ollut säädetty transseksuaalin sukupuolen vahvistamisesta annetun lain 5 §:stä poikkeavalla tavalla. Puolustusvoimien määräys, johon valituksenlainen päätös oli perustunut, oli siten tältä osin ristiriidassa lain kanssa ja sen soveltaminen oli johtanut valittajan tilanteessa siihen, ettei valittajaa ollut päätöstä tehdessä kohdeltu lain edellyttämällä tavalla hänen sukupuolensa mukaisesti ja valituksenalainen päätös tosiasiallisesti johti siten sukupuoli-identiteettiin tai sukupuolen ilmaisuun perustuvaan välittömään syrjintään. Asiassa ei tullut selvitetyksi, että menettely johtuisi muusta hyväksyttävästä seikasta kuin valittajan sukupuolesta. Hallinto-oikeus totesi edellä mainittu huomioiden, ettei aluetoimisto voinut siten hylätä valittajan hakemusta valituksenalaisessa päätöksessä esitetyillä perusteilla. Valituksenalainen päätös oli siten kumottava lainvastaisena ja asia oli palautettava aluetoimistolle uudelleen käsiteltäväksi. Selvyyden vuoksi hallinto-oikeus totesi, että asian uudelleen käsittelyssä oli sovellettava uuden käsittelyn aikana voimassa olevaa lainsäädäntöä.


Sovelletut oikeusohjeet

Perusteluissa mainitut

Suomen perustuslaki 22 § ja 107 §

Tasa-arvolaki 7 § 1 mom. ja 9 a §

Asevelvollisuuslaki (425/1950) 3 § 1 mom. (1196/1988) ja 2 mom.

Asevelvollisuuslaki (1438/2007) 2 § 1 mom. ja 3 mom., 49 § 1 mom. ja 2 mom.

Naisten vapaaehtoisesta asepalveluksesta annettu laki 1 § 1 mom., 4 § 1 mom., 5 § 1 mom.

Hallintolaki 6 §


Asian ovat ratkaisseet hallinto-oikeustuomarit Jukka Korolainen, Timo Tervonen, Annika Tikkanen ja Toni Nykänen, joka on myös esitellyt asian.


Äänestetty 3 - 1

Perusteluista eri mieltä ollut hallinto-oikeustuomari Jukka Korolainen katsoi, että päätös tuli kumotuksi sillä ensisijaisella perusteella, että valittajan tahdonvastainen reservissä pitäminen ei perustunut lakiin kuten Suomen perustuslain 2 §:n 3 momentti edellytti ja palautti asian aluetoimistolle uudelleen käsiteltäväksi valittajan reservistä poistamista varten.

Päätös on lainvoimainen.

""";
}
