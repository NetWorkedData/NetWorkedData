#import "NWEUnitySingletonManager.h"
#include "NWEHookBridge.h"
@implementation NWEUnitySingletonManager

+ (id)sharedInstance {
    static NWEUnitySingletonManager * kSharedInstance;
    @synchronized(self) {
        if (kSharedInstance == nil)
            {
            kSharedInstance = [[self alloc] init];
            }
    }
    return kSharedInstance;
}

- (id)init {
    if (self = [super init]) {
        [self setReady:NO];
    }
    return self;
}

- (void)dealloc {
}

-(void)applicationDidFinishLaunching:(NSNotification *)notification
{
    NSLog(@"#### applicationDidLaunched notification");
}


-(void)handleAppleEvent:(NSAppleEventDescriptor *)event withReplyEvent:(NSAppleEventDescriptor *)replyEvent {
    NSString *urlString = [[event paramDescriptorForKeyword:keyDirectObject] stringValue];
    
    NSLog(@"RECEIPT INTERNAL URL %@", urlString);
    
    NSAlert *tAlert = [[NSAlert alloc] init];
    [tAlert setMessageText:@"URL SCHEME RECEIPT"];
    [tAlert setInformativeText:urlString];
    [tAlert addButtonWithTitle:@"OK"];
    [tAlert setAlertStyle:NSAlertStyleWarning];
    /*NSModalResponse tResult =*/ [tAlert runModal];
    
    if ([[NWEUnitySingletonManager sharedInstance] Ready]==YES)
        {
//        NSLog(@"SEND URL TO SINGLETON");
        SingletonSendMessage("NWEUnitySingleton", "OnURLScheme",  [urlString UTF8String]);
        }
    else
        {
//        NSLog(@"MEMORIZE URL AND WAITING");
        [[NWEUnitySingletonManager sharedInstance] setURLQueue:urlString];
            // Will be sent when sharedInstance will be ready
        }
}

extern "C" {
    void _NWE_SingletonReady() {
//        NSAlert *tAlert = [[NSAlert alloc] init];
//        [tAlert setMessageText:@"NWEUnitySingletonManager"];
//        [tAlert setInformativeText:@"I am ready"];
//        [tAlert addButtonWithTitle:@"OK"];
//        [tAlert setAlertStyle:NSAlertStyleWarning];
//        NSModalResponse tResult = [tAlert runModal];
        [[NWEUnitySingletonManager sharedInstance] setReady:YES];
//        NSLog(@"_NWE_SingletonReady is ready");
            // If URLScheme is in waiting ... send it
        if ([[NWEUnitySingletonManager sharedInstance] URLQueue]!=nil)
            {
                // YES URLScheme was waiting to do
//            CallbackMethod("NWEUnitySingleton", "OnURLScheme", [[[NWEUnitySingletonManager sharedInstance] URLQueue] UTF8String]);
            SingletonSendMessage("NWEUnitySingleton", "OnURLScheme", [[[NWEUnitySingletonManager sharedInstance] URLQueue] UTF8String]);
            [[NWEUnitySingletonManager sharedInstance] setURLQueue:nil];
            }
    }
}
@end
