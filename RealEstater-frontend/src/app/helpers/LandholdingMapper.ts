import { FormGroup } from "@angular/forms";
import { LandholdingModel } from "../models/LandholdingModel";
import { FeatureModel } from "../models/FeatureModel";

export default class LandholdingMapper {
    static createLandholding(landholdingForm: FormGroup<any>, features: any[], pictures: any[], landholdingId: number = 0, isActive: boolean = true) {
        let landholding = new LandholdingModel();
        let featuresModel = new Array<FeatureModel>();

        for (let i = 0; i < features.length; i++) {
            featuresModel.push(features[i]);
        }

        landholding.id = landholdingId;
        landholding.isActive = isActive;
        landholding.title = landholdingForm.get('title')?.value;
        landholding.city = landholdingForm.get('city')?.value;
        landholding.address = landholdingForm.get('address')?.value;
        landholding.type = landholdingForm.get('type')?.value;
        landholding.constructionType = landholdingForm.get('materialType')?.value;
        landholding.constructionStage = landholdingForm.get('stage')?.value;
        landholding.yearOfConstruction = landholdingForm.get('yearOfConstruction')?.value;
        landholding.area = landholdingForm.get('area')?.value;
        landholding.floor = landholdingForm.get('floorOfResidence')?.value | 0;
        landholding.numberOfFloors = landholdingForm.get('numberOfFloors')?.value | 0;
        landholding.courtyard = landholdingForm.get('courtyard')?.value | 0;
        landholding.price = landholdingForm.get('price')?.value;
        landholding.description = landholdingForm.get('description')?.value;
        landholding.features = featuresModel;
        landholding.pictures = pictures;

        return landholding;
    }
}