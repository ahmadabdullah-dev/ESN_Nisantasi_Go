export type UserDto  = {
  id: string,
  profilePhotoPublicId: string 
  userName: string
  firstName: string
  lastName: string
  country: string
  department: string
  isActive: boolean
  email: string
}
export type CurrentUserDto = {
  id: string;
  userName: string;
  isActive: string;
  role: string
};