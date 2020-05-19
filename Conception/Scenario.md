# Scenario

## Partie 1 : Neuilly & Facebook

- [Auto] Premier monologue de Mo
	- *facebookDone* -> -1
	- sauvegarde
- [Interaction] Interaction avec le PC, lance le minijeu Facebook
	- *facebookDone* -> 1
	- sauvegarde
	- débloque la porte d'entrée / bloque le PC
	- Minijeu skippable avec F2

## Partie 2 : St-Maur & la SPA

- [Zone] Rencontre avec le serpent avant le taureau
	- *seenSnake* -> -1	
- [Zone] Rencontre avec le taureau avant le serpent
	- *seenBull* -> 1
- [Zone] Rencontre avec le serpent après le taureau
	- *seenSnake* -> 1
	- Skippable avec F3
- [Interaction] Rencontre Altéa
	- *metAltea* -> 1 
	- débloque la cave
	- sauvegarde et soigne
	- Skippable avec F4

## Partie 4 : La Cave aux Serpents

- [Interaction] Rencontre Orion
	- *metOrion* -> 1  
	- sauvegarde et soigne
	- Skippable avec F5

## Partie 5 : Le voyage, les vaches et Montgeron

- [Zone] Arrivée à Montgeron
	- dialogue
	- *seenMontgeron* -> 1
	- Skippable avec F6
- [Interaction] Trouver la maison
	- monologue
	- *seenHouse* -> 1
	- débloque la maison
	- Skippable avec F7

## Partie 6 : La maison

- [Auto] Libération de Maxime
	- dialogue
	- *DefeatedCerberus* -> -1
	- Enchaine sur le combat 
	- Auto-win du combat avec F9
- [Auto] Fin du combat
	- dialogue
	- *DefeatedCerberus* -> 1
	- Skippable avec F8
- [Auto] Fondu noir et rechargement