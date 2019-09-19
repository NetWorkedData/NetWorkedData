#import "NWEURLSchemeManager.h"
#import "NWEUnitySingletonManager.h"

@implementation NWEURLSchemeManager
- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation {
    NSLog(@"url received: %@", url);
    BOOL rReturn = NO;
    if (!url)
        {
        rReturn = NO;
        }
    else
        {
//        UIAlertView *tAlert = [[UIAlertView alloc] initWithTitle:@"URL SCHEME Will transfert" message:[url absoluteString] delegate:nil cancelButtonTitle:@"ok" otherButtonTitles:nil];
//       [tAlert show];
        
            // waiting scene is loaded ? But how?
        
            // perhaps waiting BTBUnitySingleton send some message to App?
        if ([[BTBUnitySingletonManager sharedInstance] Ready]==YES)
            {
            UnitySendMessage("BTBUnitySingleton", "OnURLScheme", [[url absoluteString] UTF8String]);
            }
        else
            {
            [[BTBUnitySingletonManager sharedInstance] setURLQueue:[url absoluteString]];
            }
        rReturn = YES;
        }
    return rReturn;
}
@end

IMPL_APP_CONTROLLER_SUBCLASS(BTBURLSchemeManager)
