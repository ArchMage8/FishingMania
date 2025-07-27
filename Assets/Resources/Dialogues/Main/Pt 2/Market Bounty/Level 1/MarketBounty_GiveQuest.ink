EXTERNAL SetVariables()
EXTERNAL SubmitQuest()
EXTERNAL SetActiveQuest()

VAR hasActiveQuest = false
VAR correspondingNPC = false
VAR Success = false

~ SetVariables()   

+{hasActiveQuest && correspondingNPC} -> Submit
+{hasActiveQuest && !correspondingNPC} -> Busy
+{!hasActiveQuest && !correspondingNPC} -> Start

=== Start ===
Hey are you still waiting for that shipment?

Well most of them have arrived, but I'm missing just one...
They don't normally arrive late, could they haven't lost or something?

    +[Could I help somehow?]
    ->QuestPrompt
    
    +[Ah, I'll leave you to it then]
    ->GenericExit

=== QuestPrompt ===
How? Do you know how to get fish or something?

Well I do know how to fish...

Hmm I'm not to sure about this...
What if something goes wrong?

    +[Relax, I'll get the job done]
    ->QuestStart
    
    +[Guess I'll leave it to you then]
    ->GenericExit

=== QuestStart ===
Ooh alright then!
I need 2 of //insert fish type

~ SetActiveQuest()

I'll be back in a Jiffy!
->END

=== Busy ===
Hey are you still waiting for that shipment?

Well most of them have arrived, but I'm missing just one...
They don't normally arrive late, could they haven't lost or something?

    +[Could I help somehow?]
    ->BusyA
    
    +[Ah, I'll leave you to it then]
    ->GenericExit


=== BusyA ===
You look busy right now, maybe finish what you have to do, dont want to keep people waiting to long...

Hmmm....
->END


=== Submit ===
You have the fish?
    +[Check it out]
    ~ SubmitQuest()
    ->SumbitCheck
    
    +[Ah, not yet]
    ->GenericExit

=== SumbitCheck ===
{Success == true:
    -> SubmitComplete
- else:
    -> SumbitIncomplete
}

=== SubmitComplete ===
Ah exactly what I need!
And not a moment too soon...
Here I'll give you 3 times the normal price of the fish

Wow thats alot!

Well thats the rate we usually charge people for special requests!
Thanks again by the way!
->END

=== SumbitIncomplete ===
Ehm?
Where is it exactly?
->END

=== GenericExit ===
Hmmm....
->END