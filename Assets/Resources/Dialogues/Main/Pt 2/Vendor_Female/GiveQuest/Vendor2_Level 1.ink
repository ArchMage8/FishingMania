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
Hey friend! #speaker: Hina #trigger: Start

Who me? #speaker: Iris #trigger: next

Yes you! Willing to help me out? #speaker: Hina #trigger: next
    +[Sure whats up?]
    ->QuestStart
    +[Umm, not right now.]
    ->Exit1
    
=== QuestStart ===
So, I've been selling so much food, and I just realised I ran out of some ingredients... 

Can you get me 2 Spring Onions?
~ SetActiveQuest()

Sure! I'll be back soon! #speaker: Iris #trigger: next
->END

=== Busy ===
Hey friend! #speaker: Hina #trigger: next
Ah wait, you look like you have something to do, ignore what I just said.
->END


=== Submit ===
Did you get my 2 Spring Onions?? #speaker: Hina #trigger: next
    +[Sure do!]
    ~ SubmitQuest()
    ->SumbitCheck
    
    +[Ah, Not yet.]
    ->Exit1

=== SumbitCheck ===
{Success == true:
    -> SubmitComplete
- else:
    -> SumbitIncomplete
}

=== SubmitComplete === 
->END

=== SumbitIncomplete ===
Ehm, is it hidden in your bag? #speaker: Hina
Because I can't see it...
->END

=== Exit1 ===
Ok then... #speaker: Hina
->END