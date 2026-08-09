import { useMutation } from "@tanstack/react-query";
import agent from "../api/agent";
import type { RegisterUserDto } from "../types/admin";

export function useRegisterAdmin() {
  return useMutation({
    mutationFn: async (creds: RegisterUserDto) => {
      const response = await agent.post("/admin/register-admin", creds);
      return response.data;
    },
  });
}

export function useRegisterMember() {
  return useMutation({
    mutationFn: async (creds: RegisterUserDto) => {
      const response = await agent.post("/admin/register-member", creds);
      return response.data;
    },
  });
}
