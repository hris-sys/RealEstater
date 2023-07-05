export default class Common {
    static regexForWebsite: string = "^(http[s]?:\\/\\/(www\\.)?|ftp:\\/\\/(www\\.)?|www\\.){1}([0-9A-Za-z-\\.@:%_\+~#=]+)+((\\.[a-zA-Z]{2,3})+)(/(.)*)?(\\?(.)*)?";
    static regexForPhone: RegExp = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/;
    static regexForPassword: RegExp = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=])(?!.*\s).{8,}$/;
    static regexForAddress: RegExp = /^[a-zA-Z\s]+[0-9]*$/;
    static regexForYear: RegExp = /^(18\d{2}|19\d{2}|20[0-2]\d|20[3-9]0)$/;
}