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
Hmm? Did you need something? #speaker: Takeshi #trigger: Start
    +[Can we go to the other islands now?]
    ->Start1
    
    +[Nevermind...]
    ->GenericExit

=== Start1 ===
Do you have your ticket? #speaker: Takeshi

Well, about that... #speaker: Iris #trigger: next

Look, you need that ticket to use the boat, it pays for the upkeep and dock fees after all... #speaker: Takeshi #trigger: next

    +[Where can I get a ticket]
    ->Start2

=== Start2 ===
There should be worker in dark blue clothes somewhere around here... #speaker: Takeshi
Just tell him you want to buy a travel ticket.

Ah gotcha! #speaker: Iris #trigger: next

So... Are you gonna go get it now? #speaker: Takeshi #trigger: next
Cuz if so, I'll start prepping the boat. 
    
    +[Yes! I'll be back soon]
    ->StartPrompt
    
    +[Actually, not right now...]
    ->GenericExit


=== StartPrompt ===
Alright, You need just 1 ticket by the way! #speaker: Takeshi #trigger: next

~ SetActiveQuest()

Sure! Be back soon! #speaker: Iris #trigger: next
->END

=== Submit ===
Ah your back, ready to go? #speaker: Takeshi #trigger: Submit
    +[Yep, lets go!]
    ->Sumbit1
    
    +[Ehm, where can I get a ticket again?]
    ->QueryExit
    
    +[Actually, just a sec]
    ->GenericExit

=== Sumbit1 ===
You got the ticket? #speaker: Takeshi 
    +[Got it right here!]
    ~ SubmitQuest()
    ->SumbitCheck
    
    +[Actually not yet...]
    ->GenericExit

=== SubmitComplete ===
Great! Lets go then! #speaker: Takeshi
->END

=== SubmitIncomplete ===
Ehm... Where is it? #speaker: Takeshi
->END

=== Busy ===
Hmm? Did you need something? #speaker: Takeshi #trigger: Busy
    +[Can we go to the other islands now?]
    ->Busy1
    
    +[Nevermind...]
    ->GenericExit


=== Busy1 ===
Ehm... You look busy, maybe finish what you have to do first, then come back #speaker: Takeshi
->END

=== GenericExit ===
Hmm... Ok then! #speaker: Takeshi #trigger: Exit1
->END

=== SumbitCheck ===
{Success == true:
    -> SubmitComplete
- else:
    -> SubmitIncomplete
}

=== QueryExit ===
There should be worker in dark blue clothes somewhere around here... #speaker: Takeshi #trigger: Exit2
Just tell him you want to buy a travel ticket.

Ah gotcha! #speaker: Iris #trigger: next
->END

