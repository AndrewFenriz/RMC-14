rmc-execution = Execute
rmc-executed = [color=#99FFFF]{ CAPITALIZE(SUBJECT($victim)) } { GENDER($victim) ->
    [epicene] seem
    *[other] seems
  } to have had { POSS-ADJ($victim) } brain removed violently.[/color]

rmc-execute-start-self = You aim the {$gun} at {$target}'s head!
rmc-execute-start-others = {$user} aims their {$gun} at {$target}'s head!

rmc-execute-cancel-dead = You decide to not execute {$target}, as they are already far beyond revival.
rmc-execute-cancel-self = You decide to not execute {$target}.

rmc-execute-final-popup = {$victimNameUpper} WAS EXECUTED BY {$userNameUpper}!
rmc-execute-final-chat = [bold][font size=24][color=red]{$victimNameUpper} WAS EXECUTED BY {$userNameUpper}![/color][/font][/bold]
