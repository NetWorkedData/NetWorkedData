//
//  Copyright 2017  Kortex
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

// [CONVENTION] Convention of layout for all our unity project in C#!
// [CONVENTION] Design Pattern
// [CONVENTION] Class suffix by design pattern CONTART convention 
// [CONVENTION] xxxxxData
// [CONVENTION] don't use xxxxxModel
// [CONVENTION] xxxxxController
// [CONVENTION] xxxxxView
// [CONVENTION] xxxxxProvider
// [CONVENTION] xxxxxService
// [CONVENTION] xxxxxRequestor

//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

// [CONVENTION] use delimiter by namespace
// [CONVENTION] use delimeter around precompile macro if necessary
//=====================================================================================================================
// [CONVENTION] namespace all files in projet
namespace NetWorkedData
{
	// [CONVENTION] use delimiter by class
	// [CONVENTION] use delimeter around precompile macro if necessary
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	#if UNITY_EDITOR
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	// [CONVENTION] HeaderDoc for enum is compulsory indication
	/// <summary>
	/// NWD layout enum.
	/// </summary>
	public enum NWDLayoutEnum {
		/// <summary>
		/// The value a.
		/// </summary>
		ValueA, // [CONVENTION] HeaderDoc for each enum value is compulsory indication
		/// <summary>
		/// The value b.
		/// </summary>
		ValueB,
		// [CONVENTION] Allways add none value

		None
	}

	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

	// [CONVENTION] HeaderDoc for class is compulsory indication
	public partial class NWDLayoutClass //[CONVENTION] In ChamelCase, start by TRIGRAM of project or package
	{
		// [CONVENTION] use delimiter by region properties group and for all methods
		// [CONVENTION] use delimeter around precompile macro if necessary
		//-------------------------------------------------------------------------------------------------------------

		#region Const

		//-------------------------------------------------------------------------------------------------------------
		// Insert constants here
		// [CONVENTION] all public before private
		//-------------------------------------------------------------------------------------------------------------
		// [CONVENTION] Use convention : in uppercase separated by _ start by K_ for public
		// [CONVENTION] HeaderDoc for const is compulsory indication
		/// <summary>
		/// The K_MY_CONSTANT
		/// </summary>
		public const int K_MY_CONSTANT =1 ;
		// [CONVENTION] Use convention : in uppercase separated by _ start by C_ for private
		/// <summary>
		/// The C_MY_CONSTANT is private
		/// </summary>
		private const int C_MY_CONSTANT = 1;

		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Class properties

		//-------------------------------------------------------------------------------------------------------------
		// Insert static properties here
		// [CONVENTION] all public before private
		//-------------------------------------------------------------------------------------------------------------
		// [CONVENTION] Use convention : in CamelCase start by k if public
		// [CONVENTION] HeaderDoc for properties is compulsory indication
		/// <summary>
		/// The public shared instance.
		/// </summary>
		public static NWDLayoutClass kSharedInstance;
		// [CONVENTION] Use convention : in CamelCase start by c if private
		// [CONVENTION] HeaderDoc for properties is compulsory indication
		/// <summary>
		/// The private shared instance.
		/// </summary>
		private static NWDLayoutClass cSharedInstance;
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Instance properties

		//-------------------------------------------------------------------------------------------------------------
		// Insert instance properties here
		// [CONVENTION] all public before private
		// [CONVENTION] property suffix by User usage
		// [CONVENTION] xxxxxButton
		// [CONVENTION] xxxxxLabel
		// [CONVENTION] xxxxxPanel
		// [CONVENTION] xxxxxSprite
		// [CONVENTION] xxxxxGameObject
		// [CONVENTION] property suffix for specil type 
		// [CONVENTION] xxxxxList
		// [CONVENTION] xxxxxDictionary (use name with value/key binding)
		// [CONVENTION] xxxxxArray
		//-------------------------------------------------------------------------------------------------------------
		// [CONVENTION] Use convention : in CamelCase Start by lowerCase no prefix if public
		// [CONVENTION] HeaderDoc for properties is compulsory indication
		/// <summary>
		/// My properties.
		/// </summary>
		public bool activity = true;
		// [CONVENTION] Use convention : in CamelCase start by lowerCase prefix by '_' if private 
		// [CONVENTION] HeaderDoc for properties is compulsory indication
		/// <summary>
		/// Another property but private.
		/// </summary>
		private bool _anotherProperty;
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Class methods

		//-------------------------------------------------------------------------------------------------------------
		// Insert class methods here
		// [CONVENTION] all public before private
		//-------------------------------------------------------------------------------------------------------------
		// [CONVENTION] Use convention : in CamelCase Start by UpperCase for public
		// [CONVENTION] HeaderDoc for methods is compulsory indication
		/// <summary>
		/// My method with parameter.
		/// </summary>
		/// <returns><c>true</c>, if method with parameter was myed, <c>false</c> otherwise.</returns>
		/// <param name="sSentData">If set to <c>true</c> s sent data.</param>
		public static bool MyMethodWithParam (bool sSentData) // [CONVENTION] sent variable in CamelCase by prefix 's'
		{
			bool rReturn = false;
			bool tTemporaryValue = sSentData; // [CONVENTION] temporary variable in CamelCase by prefix 't'

			// [CONVENTION] no Ternary 
			if (sSentData == true) { // CONVENTION] use == true or == false, don't use default analyze
				rReturn = false;
			} else {
				rReturn = true;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		// Insert static methods here
		// [CONVENTION] Use convention : in CamelCase Start by UpperCase for private
		// [CONVENTION] HeaderDoc for methods is compulsory indication
		/// <summary>
		/// My method.
		/// </summary>
		/// <returns><c>true</c>, if method was my, <c>false</c> otherwise.</returns>
		private static bool _MyMethod ()
		{
			
			bool rReturn = false; // [CONVENTION] return variable in CamelCase by prefix 'r'
			// [CONVENTION] add comment for all important part
			// [CONVENTION] use TODO and FIXME task as soosn as possible
			// TODO : write this method 
			// FIXME : fix this method
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Instance methods

		//-------------------------------------------------------------------------------------------------------------
		// Insert instance methods here
		// [CONVENTION] all public before private

		// [CONVENTION] method suffix by Interface Connexion
		// [CONVENTION] xxxxxAction if is use as interface connexion action

		//-------------------------------------------------------------------------------------------------------------
		// [CONVENTION] HeaderDoc for methods is compulsory indication
		/// <summary>
		/// My the method.
		/// </summary>
		/// <returns><c>true</c>, if method was my, <c>false</c> otherwise.</returns>
		/// <param name="sSentData">If set to <c>true</c> s sent data.</param>
		public bool MyMethod (bool sSentData) // [CONVENTION] sent variable in CamelCase by prefix 's'
		{
			bool rReturnData = false;
			bool tTemporaryData = !sSentData;
			if (tTemporaryData == false) {
				rReturnData = true;
			}
			// [CONVENTION] use this separator for macro precompile
			//--------------
			#if UNITY_EDITOR
			//--------------
			//  recalculate all sign possibilities
			rReturnData = true;
			//--------------
			#endif
			//--------------

			return rReturnData;
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	#endif
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================