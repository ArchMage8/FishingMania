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
Hey, are you still waiting for that shipment?

Well, most of them have arrived, but I'm missing just one...
They don't normally arrive late; I hope they didn't get lost or something.

    +[Can I help somehow?]
    ->QuestPrompt
    
    +[Ah, I'll leave you to it then.]
    ->GenericExit

=== QuestPrompt ===
How? Do you know how to get fish or something?

Well, I do know how to fish...

Hmm, I'm not too sure about this...
What if something goes wrong?

    +[Relax, I'll get the job done.]
    ->QuestStart
    
    +[Guess I'll leave it to you then.]
    ->GenericExit

=== QuestStart ===
Oh alright then!
I need 2 Red Snappers!

~ SetActiveQuest()

I'll be back in a Jiffy!
->END

=== Busy ===
Hey, are you still waiting for that shipment?

Well, most of them have arrived, but I'm missing just one...
They don't normally arrive late; I hope they didn't get lost or something.

    +[Can I help somehow?]
    ->BusyA
    
    +[Ah, I'll leave you to it then.]
    ->GenericExit


=== BusyA ===
You look busy right now, maybe finish what you have to do first. Don't wanna keep people waiting for too long...

Hmmm...
->END


=== Submit ===
You have the fish?
    +[Yes! Check it out!]
    ~ SubmitQuest()
    ->SumbitCheck
    
    +[Ah, not yet.]
    ->GenericExit

=== SumbitCheck ===
{Success == true:
    -> SubmitComplete
- else:
    -> SumbitIncomplete
}

=== SubmitComplete ===
Ah, exactly what I needed!
And not a moment too soon...
Here I'll give you 3 times the normal price of the fish.

Wow that's alot!

Well that's the rate we usually charge people for special requests!
Thanks again by the way!
->END

=== SumbitIncomplete ===
Ehm?
Doesn't look like you have the fish I needed.
I need 2 Red Snappers
->END

=== GenericExit ===
Hmmm....
->END