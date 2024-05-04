using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects.ChannelDto;
using Shared.DataTransferObjects.ChannelMessageDto;
using Shared.DataTransferObjects.ChatDto;
using Shared.DataTransferObjects.ChatMessageDto;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class ChannelService : IChannelService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<ChannelDto> _dataShaper;

        public ChannelService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<ChannelDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        public async Task<(IEnumerable<ExpandoObject> channels, MetaData metaData)> GetChannelsAsync(ChannelParameters channelParameters, bool trackChanges)
        {
            var channelsWithMetaData = await _repository.Channel.GetChannelsAsync(channelParameters, trackChanges);
            var channelsDto = _mapper.Map<IEnumerable<ChannelDto>>(channelsWithMetaData);
            var shapedData = _dataShaper.ShapeData(channelsDto, channelParameters.Fields);


            return (channels: shapedData, metaData: channelsWithMetaData.MetaData);
        }
        public async Task<(IEnumerable<ExpandoObject> channels, MetaData metaData)> GetAccountChannelsAsync(Guid accountId, ChannelParameters channelsParameters, bool trackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);

            var channelsWithMetaData = await _repository.Channel.GetAccountChannelsAsync(accountId, channelsParameters);
            var channelsDto = _mapper.Map<IEnumerable<ChannelDto>>(channelsWithMetaData);
            var shapedData = _dataShaper.ShapeData(channelsDto, channelsParameters.Fields);


            return (channels: shapedData, metaData: channelsWithMetaData.MetaData);
        }

        public async Task<IEnumerable<ChannelDto>> GetAccountChannelsAsync(string accountId)
        {
            var channels = await _repository.Channel.GetAccountChannelsAsync(accountId);
            return _mapper.Map<IEnumerable<ChannelDto>>(channels);
        }

        public async Task<ChannelDto> GetChannelAsync(Guid channelId, bool trackChanges)
        {
            var channel = await _repository.Channel.GetChannelAsync(channelId, trackChanges);

            if (channel is null) throw new ChannelNotFoundException(channelId);

            var channelDto = _mapper.Map<ChannelDto>(channel);

            channelDto.MembersAccounts = _mapper.Map<IEnumerable<ChannelMemberDto>>(await _repository.Channel.GetChannelMembersAccountsAsync(channelId, trackChanges));

            foreach (var member in channelDto.MembersAccounts)
            {
                var channelMember = await _repository.Channel.GetChannelMemberAsync(member.Id, channel.Id);
                member.ChannelRole = channelMember.ChannelRole;
            }
            return channelDto;
        }

        public async Task<ChannelDto> CreateChannelAsync(Guid accountId, ChannelForCreationDto channelForCreation, bool trackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);
            var channel = _mapper.Map<Channel>(channelForCreation);
            if (channelForCreation.Image != null)
            {
                var result = await _repository.Photo.AddPhotoAsync(channelForCreation.Image);
                channel.Image = result.Url.ToString();
            }
            _repository.Channel.CreateChannel(channel, account);
            await _repository.SaveAsync();
            var channelToReturn = _mapper.Map<ChannelDto>(channel);
            return channelToReturn;
        }

        public async Task DeleteChannelAsync(Guid channelId, bool trackChanges)
        {
            var channel = await _repository.Channel.GetChannelAsync(channelId, trackChanges);
            if (channel is null) throw new ChannelNotFoundException(channelId);
            if (channel.Image != null) await _repository.Photo.DeletePhotoAsync(channel.Image);
            _repository.Channel.DeleteChannel(channel);
            await _repository.SaveAsync();
        }

        public async Task UpdateChannelAsync(Guid channelId, ChannelForUpdateDto channelForUpdate, bool channelTrackChanges, bool accountTrackChanges)
        {
            var channel = await _repository.Channel.GetChannelAsync(channelId, channelTrackChanges);
            if (channel is null) throw new ChannelNotFoundException(channelId);
            if (channelForUpdate.Image != null)
            {
                if (channel.Image != null) await _repository.Photo.DeletePhotoAsync(channel.Image);
                var result = await _repository.Photo.AddPhotoAsync(channelForUpdate.Image);
                channel.Image = result.Url.ToString();
            }
            _mapper.Map(channelForUpdate, channel);
            await _repository.SaveAsync();
        }

        public async Task LeaveChannelAsync(Guid accountId, Guid channelId, bool trackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, trackChanges);
            if (account is null) throw new AccountNotFoundException(accountId);
            var channel = await _repository.Channel.GetChannelAsync(channelId, trackChanges);
            if (channel is null) throw new ChannelNotFoundException(channelId);
            var channelMember = await _repository.Channel.GetChannelMemberAsync(accountId, channelId);
            if (channelMember is null) throw new ChannelMemberNotFoundException(accountId, channelId);
            _repository.Channel.DeleteChannelMember(channelMember);
            if (channelMember.ChannelRole == ChannelRoles.Admin) _repository.Channel.DeleteChannel(channel);
            await _repository.SaveAsync();
        }

        public async Task<(IEnumerable<ChannelMessageDto> channelMessages, MetaData metaData)> GetChannelMessagesAsync(Guid channelId, ChannelMessageParameters channelMessageParameters, bool trackChanges)
        {
            var channel = await _repository.Channel.GetChannelAsync(channelId, trackChanges);
            if (channel is null) throw new ChannelNotFoundException(channelId);
            var channelMessagesWithMetaData = await _repository.ChannelMessage.GetChannelMessagesAsync(channelId, channelMessageParameters, trackChanges);
            var channelMessagesDto = _mapper.Map<IEnumerable<ChannelMessageDto>>(channelMessagesWithMetaData);
            return (channelMessages: channelMessagesDto, metaData: channelMessagesWithMetaData.MetaData);
        }
        public async Task<ChannelMessageDto> CreateChannelMessageAsync(Guid channelId, ChannelMessageForCreationDto messageForCreation, bool trackChanges)
        {
            var channel = await _repository.Channel.GetChannelAsync(channelId, trackChanges);
            if (channel is null) throw new ChannelNotFoundException(channelId);
            var message = _mapper.Map<ChannelMessage>(messageForCreation);
            if (messageForCreation.Image != null)
            {
                var result = await _repository.Photo.AddPhotoAsync(messageForCreation.Image);
                message.Image = result.Url.ToString();
            }
            _repository.ChannelMessage.CreateChannelMessage(channelId, message);
            await _repository.SaveAsync();

            var channelMessagetoReturn = _mapper.Map<ChannelMessageDto>(message);

            return channelMessagetoReturn;
        }
        public async Task DeleteChannelMessageAsync(Guid channelMessageId, bool trackChanges)
        {
            var channelMessage = await _repository.ChannelMessage.GetChannelMessageAsync(channelMessageId, trackChanges);
            if (channelMessage is null) throw new ChannelMessageNotFoundException(channelMessageId);
            if (channelMessage.Image != null) await _repository.Photo.DeletePhotoAsync(channelMessage.Image);
            _repository.ChannelMessage.DeleteChannelMessage(channelMessage);
            await _repository.SaveAsync();
        }

        public async Task<ChannelMessageDto> GetChannelMessageAsync(Guid messageId, bool trackChanges)
        {
            var message = await _repository.ChannelMessage.GetChannelMessageAsync(messageId, trackChanges);
            if (message is null) throw new ChannelMessageNotFoundException(messageId);
            var messageDto = _mapper.Map<ChannelMessageDto>(message);
            return messageDto;
        }

        public async Task AddMemberToChannelAsync(Guid accountId, Guid channelId, bool channelTrackChanges, bool accountTrackChanges)
        {
            var account = await _repository.Account.GetAccountAsync(accountId, accountTrackChanges);
            if(account is null) throw new AccountNotFoundException(accountId);
            var channel = await _repository.Channel.GetChannelAsync(channelId, channelTrackChanges);
            if(channel is null) throw new ChannelNotFoundException(channelId);
            channel.ChannelMembers.Add(new ChannelMember { ChannelId = channelId, AccountId = accountId, ChannelRole = ChannelRoles.Member });
            await _repository.SaveAsync();
        }
    }
}
