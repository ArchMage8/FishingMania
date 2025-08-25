EXTERNAL SetVariables()
EXTERNAL SubmitQuest()
EXTERNAL SetActiveQuest()

VAR hasActiveQuest = false
VAR correspondingNPC = false
VAR Success = false

~ SetVariables()   

+{hasActiveQuest && correspondingNPC} -> Submit
+{hasActiveQuest && !correspondingNPC} -> Busy
+{!hasActiveQuest} -> Start

=== Start ===
Hmm? Did you need something?
    +[Can we go to the other islands now?]
    ->Start1
    
    +[Nevermind...]
    ->GenericExit

=== Start1 ===
Do you have your ticket?

Well, about that....

Look, you need that ticket to use the boat, it pays for the upkeep and dock fees after all...

    +[Where can I get a ticket]
    ->Start2

=== Start2 ===
There should be worker in dark blue clothes somewhere around here...
Just tell him you want to buy a travel ticket.

Ah gotcha!

So... Are you gonna go get it now?
Cuz if so, I'll start prepping the boat.
    
    +[Yes! I'll be back soon]
    ->StartPrompt
    
    +[Actually, not right now...]
    ->GenericExit


=== StartPrompt ===
Alright, You need just 1 ticket by the way!

~ SetActiveQuest()

Sure! Be back soon!
->END

=== Submit ===
Ah your back, ready to go?
    +[Yep, lets go!]
    ->Sumbit1
    
    +[Ehm, where can I get a ticket again?]
    ->QueryExit
    
    +[Actually, just a sec]
    ->GenericExit

=== Sumbit1 ===
You got the ticket?
    +[Got it right here!]
    ~ SubmitQuest()
    ->SumbitCheck

=== SubmitComplete ===
Great! Lets go then!
->END

=== SubmitIncomplete ===
Ehm... Where is it?
->END

=== Busy ===
Hmm? Did you need something?
    +[Can we go to the other islands now?]
    ->Busy1
    
    +[Nevermind...]
    ->GenericExit


=== Busy1 ===
Ehm... You look busy, maybe finish what you have to do first, then come back
->END

=== GenericExit ===
Hmm... Ok then!
->END

=== SumbitCheck ===
{Success == true:
    -> SubmitComplete
- else:
    -> SubmitIncomplete
}

=== QueryExit ===
There should be worker in dark blue clothes somewhere around here...
Just tell him you want to buy a travel ticket.

Ah gotcha!
->END

