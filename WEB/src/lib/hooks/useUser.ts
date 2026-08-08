import { useQuery } from "@tanstack/react-query";
import agent from "../api/agent";
import type { UserDto } from "../types/user";

export const useCurrentUser = () =>
  useQuery<UserDto>({
    queryKey: ["currentUser"],
    queryFn: () => agent.get<UserDto>("/User/current").then((res) => res.data),
    staleTime: 5 * 60 * 1000, // 5 min
    retry: false,
  });

export const useGetUserByUsername = (userName: string) =>
  useQuery<UserDto>({
    queryKey: ["user", userName],
    queryFn: () =>
      agent
        .get<UserDto>("/User", { params: { userName } })
        .then((res) => res.data),
    enabled: !!userName,
    staleTime: 1 * 60 * 1000, // 1 min
    retry: false,
  });
