/*
 * Copyright (c) 2024 OmnInteractive Solutions. All rights reserved.
 */

using System;
using UnityEngine;

namespace UTeleApp
{
    [Serializable]
    public class BiometricManager
    {
        public bool isInited; // Shows whether biometrics object is initialized.
        public bool isBiometricAvailable; // Shows whether biometrics is available on the current device.
        public string biometricType; // The type of biometrics currently available on the device. Can be one of: finger, face, unknown.
        public bool isAccessRequested; // Shows whether permission to use biometrics has been requested.
        public bool isAccessGranted; // Shows whether permission to use biometrics has been granted.
        public bool isBiometricTokenSaved; // Shows whether the token is saved in secure storage on the device.
        public string deviceId; // A unique device identifier that can be used to match the token to the device.

        /// <summary>
        /// Bot API 7.2+ A method that initializes the BiometricManager object.
        /// It should be called before the object's first use.
        /// If an optional callback parameter was passed, the callback function will be called when the object is initialized.
        /// </summary>
        public BiometricManager Init()
        {
            TelegramWebApp.InvokeMethod("BiometricManager.init");
            return this;
        }

        /// <summary>
        /// Bot API 7.2+ A method that requests permission to use biometrics according to the params argument of type BiometricRequestAccessParams.
        /// If an optional callback parameter was passed, the callback function will be called and the first argument will be a boolean indicating whether the user granted access.
        /// </summary>
        public BiometricManager RequestAccess(BiometricRequestAccessParams parameters)
        {
            if (!this.isInited)
            {
                return this;
            }
            TelegramWebApp.InvokeMethodWithJsonStringParam(
                "BiometricManager.requestAccess",
                JsonUtility.ToJson(parameters)
            );
            return this;
        }

        /// <summary>
        /// Bot API 7.2+ A method that authenticates the user using biometrics according to the params argument of type BiometricAuthenticateParams.
        /// If an optional callback parameter was passed, the callback function will be called and the first argument will be a boolean indicating whether the user authenticated successfully.
        /// If so, the second argument will be a biometric token.
        /// </summary>
        public BiometricManager Authenticate(BiometricAuthenticateParams parameters)
        {
            if (!this.isInited || !this.isAccessGranted)
            {
                return this;
            }
            TelegramWebApp.InvokeMethodWithJsonStringParam(
                "BiometricManager.authenticate",
                JsonUtility.ToJson(parameters)
            );
            return this;
        }

        /// <summary>
        /// Bot API 7.2+ A method that updates the biometric token in secure storage on the device.
        /// To remove the token, pass an empty string.
        /// If an optional callback parameter was passed, the callback function will be called and the first argument will be a boolean indicating whether the token was updated.
        /// </summary>
        public BiometricManager UpdateBiometricToken(string token)
        {
            TelegramWebApp.InvokeMethodWithJsonStringParam(
                "BiometricManager.updateBiometricToken",
                token
            );
            return this;
        }

        /// <summary>
        /// Bot API 7.2+ A method that opens the biometric access settings for bots.
        /// Useful when you need to request biometrics access to users who haven't granted it yet.
        /// Note that this method can be called only in response to user interaction with the Mini App interface.
        /// </summary>
        public void OpenSettings()
        {
            TelegramWebApp.InvokeMethod("BiometricManager.openSettings");
        }
    }

    public struct BiometricRequestAccessParams
    {
        public string reason; // Optional. The text to be displayed to a user in the popup describing why the bot needs access to biometrics, 0-128 characters.
    }

    public struct BiometricAuthenticateParams
    {
        public string reason; // Optional. The text to be displayed to a user in the popup describing why you are asking them to authenticate and what action you will be taking based on that authentication, 0-128 characters.
    }
}
