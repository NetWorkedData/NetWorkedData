//
//  NWEOverrideAppDelegate.m
// NetWorkedDataOSX
//
//  Created by Jean-François CONTART on 13/06/2018.
//  Copyright © 2018 Jean-François CONTART. All rights reserved.
//
#import "NWEURLSchemeManager.h"
#import "NWEUnitySingletonManager.h"
#include "NWEHookBridge.h"
#import <Cocoa/Cocoa.h>

@implementation NWEURLSchemeManager

+(void)load
{
    NSLog(@"NWEURLSchemeManager load");
    [super load];
    [[NSAppleEventManager sharedAppleEventManager] setEventHandler:[NWEUnitySingletonManager sharedInstance] andSelector:@selector(handleAppleEvent:withReplyEvent:) forEventClass:kInternetEventClass andEventID:kAEGetURL];
    
    [[NSNotificationCenter defaultCenter] addObserver:[NWEUnitySingletonManager sharedInstance] selector:@selector(applicationDidFinishLaunching:) name:NSApplicationDidFinishLaunchingNotification object:nil];
}
@end
