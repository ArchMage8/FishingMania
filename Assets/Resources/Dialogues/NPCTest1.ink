EXTERNAL SetVariables()
EXTERNAL SubmitQuest()
EXTERNAL SetActiveQuest()

VAR hasActiveQuest = false
VAR correspondingNPC = false
VAR Success = false

// ~ SetVariables()
// ~ SubmitQuest()
// ~ SetActiveQuest()

//#speaker:npc 
//#trigger: Branch1
//#trigger: Branch2
//#trigger: Branch3

Hello there! #speaker:Sugu #trigger: Branch1
~ SetVariables()
-> 1A

=== 1A ===
{ hasActiveQuest == false:
    -> 2B
- else:
    { correspondingNPC == false:
        -> 2A
    - else:
        -> 2C
    }
}


=== 2A === // Is busy
Looks like your busy, nevermind then #speaker:Sugu #trigger: Branch1
-> END

=== 2B === // Not busy
I really need some salt, can you help me? #speaker:Sugu #trigger: Branch2
    +[Sure, let me get some]
       ~ SetActiveQuest()
       -> 3A
    
    +[Ah, im a bit busy right now]
       -> 3B

=== 2C === // Is busy and want so sumbit
    Do you have the salt? #speaker:Sugu #trigger: Branch3
    +[Here you go!]
        ~ SubmitQuest()
        -> Process2
    
    +[I don't have it yet]
        -> 3E


=== Process2 ===

{Success == true:
    -> 3C
- else:
    -> 3D
}



=== 3A === //Setting the active quest
Thank you! #speaker:Sugu #trigger: Branch1
-> END
    
=== 3B === // Reject Quest
 Nevermind then... #speaker:Sugu #trigger: Branch2
-> END

=== 3C === // quest complete
Thank you so much! #speaker:Sugu #trigger: Branch1

-> END

=== 3D === // quest fail
Um.. Where is it? #speaker:Sugu #trigger: Branch2
    -> END

=== 3E === // quest pending
 Ah, alright then! #speaker:Sugu #trigger: Branch3
        -> END










