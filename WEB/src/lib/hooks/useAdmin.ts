import { useMutation } from "@tanstack/react-query";
import agent from "../api/agent";
import type { RegisterUserDto } from "../types/admin";

export const useAdmin = () => {

  const registerAdminAsync = useMutation({
      mutationFn: async (creds: RegisterUserDto) => {
            const response = await agent.post("/admin/register-admin",creds)
            return response.data;
      }
  }) 

  const registerMemberAsync = useMutation({
      mutationFn: async (creds: RegisterUserDto) => {
            const response = await agent.post("/admin/register-member",creds)
            return response.data;
      }
  }) 
  
  return {
   registerAdminAsync,
   registerMemberAsync
  };
};