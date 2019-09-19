//
//  NWEHookBridge.m
//  NetWorkedDataOSX
//
//  Created by Jean-François CONTART on 07/06/2018.
//  Copyright © 2018 Jean-François CONTART. All rights reserved.
//

#import <Cocoa/Cocoa.h>

#include "NWEHookBridge.h"

static UnityCommandCallback kSingletonCallback = NULL;

void SingletonCallback (UnityCommandCallback sCallbackMethod){
    kSingletonCallback = sCallbackMethod;
}

void SingletonSendMessage (const char * sObjectName, const char * sCommandName, const char * sData){
    if (kSingletonCallback != NULL)
        {
        kSingletonCallback(sObjectName, sCommandName, sData);
        }
    else
        {
        NSLog(@"kSingletonCallback is null");
        }
}
