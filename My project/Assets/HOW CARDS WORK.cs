using UnityEngine;

public class HOWCARDSWORK : MonoBehaviour
{
    /* So the way I made the cards work is really weird but it makes sense for me.
     * 
     * There are basically 4 things you have to know about them.
     * 
     * 
     * 1. Effects vs Cards
     * 
     * - First of all, to add a card you want to right click the project menu (Create > Cards). 
     * - There will be an option to add a 'New Card' or there will be another option called 'Effects' that has 2 options
     * 
     * - The 'New Card' option is basically how the card will be displayed.
     * - This will include the name and description of the card along with a place to add an effect how to toggle items
     * 
     * - The 'Effects' are basically the brain of the card.
     * - They handle everything that goes on in the background of the cards and how they function.
     * - There are 2 different kinds of effects, those being AbilityUnlock and StatModifier
     * 
     * - AbilityUnlock is specifically to handle Abilities.
     * - I know that sounds self explanatory but its only for coded abilities like a dash.
     * 
     * - StatModifier allows for the players stats to be altered depending on what value is set for each.
     * - As of right now it is only number of jumps and speed but I'll explain in the other section how to add more.
     * 
     * - Make sure you make a New Card then drag the effect you made into it so it can work.
     * 
     * - Also for the effects its gonna set the time penalty to 7 everytime, just make sure you set it to 0.
     * - I only made that a thing for the downside of the speed cards.
     * 
     * 
     * 2. Enabling and Disabling Objects
     * 
     * - So under each card there is a list that can be created of IDs to be listed of what to enable/disable
     * - The check mark under the ID depends on what's happening to it (CHECKED = IT WILL BE ENABLED -- UNCHECKED = IT WILL BE DISABLED)
     * - The way you assign an ID to an object is by applying the "CardTarget" script to said object.
     * - You can look at the examples I have for the different objects but you just type a UNIQUE (very important) ID in the bar.
     * - After you typed in said ID in the "CardTarget" script in the object then you can add a box to the card list and input that ID into it.
     * - I'm not sure how you setup the weapons but if its a game object and not ONLY a script, I would use this method.
     * 
     * 
     * 3. How to add stat changes
     * 
     * - Just look in StatModifierEffect, it's pretty self explanatory but you just call the PlayerStat you're trying to change.
     * - Then you just change it using =+ or =-
     * - I can't really explain this one well but it makes sense when you look at it.
     * 
     * 
     * 4. Adding the cards to the pool
     * 
     * - This is VERY IMPORTANT if you want your cards to actually be in the game.
     * - There's a game objected called 'CardUIManager' and a subsection called 'Card Setup' with a list of 'All Possible Cards'
     * - Just add a new instance and drag a CARD (Not an Effect) into the slot. 
     * 
     * Good luck. :)
    */
}

