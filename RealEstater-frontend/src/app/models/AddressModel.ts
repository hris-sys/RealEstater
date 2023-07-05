import { CityModel } from "./CityModel";

export class AddressModel {
    id!: number;
    title!: string;
    city?: CityModel;
}