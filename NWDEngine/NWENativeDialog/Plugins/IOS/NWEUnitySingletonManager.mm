#import "NWEUnitySingletonManager.h"
@implementation NWEUnitySingletonManager
/// memory in static

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
        Ready = NO;
    }
    return self;
}

- (void)dealloc {
}

extern "C" {
    void _NWE_SingletonReady() {
//        UIAlertView *tAlert =[[UIAlertView alloc] initWithTitle:@"NWEUnitySingleton" message:@"Singleton is ready" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
//        [tAlert show];
        [[NWEUnitySingletonManager sharedInstance] setReady:YES];
        if ([[NWEUnitySingletonManager sharedInstance] URLQueue]!=nil)
            {
            UnitySendMessage("NWEUnitySingleton", "OnURLScheme", [[[NWEUnitySingletonManager sharedInstance] URLQueue] UTF8String]);
            [[NWEUnitySingletonManager sharedInstance] setURLQueue:nil];
            }
    }
}
@end
