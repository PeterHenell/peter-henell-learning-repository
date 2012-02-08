%%%-------------------------------------------------------------------
%%% File    : test.erl
%%% Author  :  <MrM@EBRA>
%%% Description : 
%%%
%%% Created :  3 Jan 2009 by  <MrM@EBRA>
%%%-------------------------------------------------------------------
-module(test).

-export([start/0, loop/1, init/0, guess/1]).


init() ->
    findSolution([0,0,6,2,0,1,0,0,0,8,0,0,0,0,0,0,7,1,0,0,1,7,0,0,0,3,2,0,0,7,0,3,0,0,4,0,0,5,0,0,0,0,0,8,0,0,8,0,0,4,0,7,0,0,4,6,0,0,0,5,8,0,0,1,7,0,0,0,0,0,0,4,0,0,0,4,0,6,5,0,0]).

% Letar efter en l�sning och returnerar den om den finns.
findSolution(Input) ->
    if 
	isFull(Input) ->
	    Input;
	true -> 
	    if
		isValid(Input, getValue(), findFreeSlot()) ->
		    setValue(Input, v�rdet, slotten);
		true ->
		    n�stav�rde()
		    
		
	    end
    end,
    Input.

% hittar en snygg plats som �r ledig att l�gga en siffra p�.
findFreeSlot(Input) ->
    0.

% kontrollerar om man f�r l�gga Number p� spelplanen Input p� plats Slot.
isValid(Input, Number, Slot) ->
    true.

% kontrollerar om f�ltet �r fullt, d� �r vi klara.
isFull(Input) ->
    true.

% Stoppar in v�rdet p� plats Slot.
setValue(Input, Number, Slot) ->
    Input.

