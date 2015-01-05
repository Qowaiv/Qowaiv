/**
 * To support JSON.stringify()
 */
interface IJsonStringifyable {
    /** 
     * Returns a JSON representation of the object.
     */
    toJSON(): string;
}