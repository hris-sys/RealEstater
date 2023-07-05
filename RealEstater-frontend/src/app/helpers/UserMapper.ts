import { FormGroup } from "@angular/forms";
import { UpdateUserInfoModel } from "../models/UpdateUserInfoModel";
import { UserModel } from "../models/UserModel";

export default class UserMapper {
    static createUser(signUpForm: FormGroup<any>) {
        let registeredUser = new UserModel();
        registeredUser.firstName = signUpForm.get('firstName')?.value;
        registeredUser.lastName = signUpForm.get('lastName')?.value;
        registeredUser.email = signUpForm.get('email')?.value;
        registeredUser.phoneNumber = signUpForm.get('phoneNumber')?.value;
        registeredUser.websiteUrl = signUpForm.get('websiteURL')?.value;
        registeredUser.password = signUpForm.get('password')?.value;
        return registeredUser;
    }
    static mapUpdateUser(editUserForm: FormGroup<any>, imageUrl: any, userEmail: string) {
        let updatedUser = new UpdateUserInfoModel();
        updatedUser.firstName = editUserForm.get('firstName')?.value;
        updatedUser.lastName = editUserForm.get('lastName')?.value;
        updatedUser.phoneNumber = editUserForm.get('phoneNumber')?.value;
        updatedUser.websiteURL = editUserForm.get('websiteURL')?.value;
        updatedUser.email = userEmail;

        if (imageUrl != null)
            updatedUser.pictureURL = imageUrl;

        return updatedUser;
    }
}